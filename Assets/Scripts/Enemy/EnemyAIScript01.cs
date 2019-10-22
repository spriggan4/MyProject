using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIScript01 : MonoBehaviour
{
    public bool isDead = false;             // 생존 여부
    public bool runAway = false;            // 도망 여부
    public bool runAwayByHp = false;        // 도망에 체력 조건 사용

    // 거리 설정
    public float moveableRadius = 30.0f;    // 움직이는 범위, 값이 0이거나 설정값 내에서만 움직임
    public float visualRadius = 20.0f;      // 시야 범위
    public float attackRange = 2.0f;        // 공격 거리
    public float runAwayDistance = 5.0f;    // 도망 시작 거리

    // 시간 설정
    public float attackTime = 2.0f;

    // 속도 설정
    public float walkSpeed = 3.0f;          // 걷는 속도
    public float runSpeed = 5.0f;           // 뛰는 속도
    public float rotationSpeed = 2.0f;      // 회전 속도

    // 웨이포인트 설정
    public Transform[] waypoints = null;    // 경로 설정
    public bool useWaypoint = false;        // 웨이포인트 사용
    public bool reversePatrol = false;       // 역순회
    public bool pauseAtWaypoints = false;   // 웨이포인트에서 멈춤 여부
    public float pauseMin = 1.0f;           // 웨이포인트 최소 일시 정지 시간
    public float pauseMax = 3.0f;           // 웨이포인트 최대 일시 정지 시간


    public float huntingTimer = 5.0f;       // 추적 지속 시간
    public bool targetOn = false;

    private Vector3 startPos;
    private Vector3 targetPos;

    public bool playerHasBeenSeen = false;     // 플레이어 발견
    private bool enemyCanAttack = false;        // 공격 할 수 있는지
    private bool enemyIsAttacking = false;

    private float delayTime = 2f;
    private float deltaTime = 0f;

    private float lastShotFired = 0f;
    private float lostPlayerTimer = 0f;
    private bool targetIsOutOfSight = false;

    private bool waypointCountdown = false;
    private int waypointPatrol = 0;
    private bool pauseWaypointControl;

    private Transform target = null;

    private CharacterStat myStats = null;
    private Animator animator = null;
    private AtkMng atkMng = null;
    private NavMeshAgent nav = null;

    private string state = "idle";

    public void SetTarget(GameObject _target)
    {
        if (_target == null) { return; }
        target = _target.transform;
    }

    void Start()
    {
        StartCoroutine(Initialize());
    }

    void Update()
    {
        AIFunctionality();
    }

    private IEnumerator Initialize()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target == null) { Debug.LogError("target is null"); }

        myStats = this.gameObject.GetComponent<EnemyStats>();
        animator = this.gameObject.GetComponentInChildren<Animator>();
        atkMng = this.gameObject.GetComponent<AtkMng>();
        nav = this.gameObject.GetComponent<NavMeshAgent>();

        this.gameObject.GetComponentInChildren<AnimationEventReceiver>().attackHit = AttackHit;

        startPos = this.transform.position;

        nav.speed = walkSpeed;
        nav.stoppingDistance = attackRange;

        yield return null;
    }

    private void AIFunctionality()
    {
        if ((!target) || (isDead)) { return; }

        isDead = myStats.IsDead;
        targetPos = target.transform.position;
        nav.stoppingDistance = 2f;

        float distance = Vector3.Distance(transform.position, targetPos);

        if (enemyIsAttacking || isDead)
        {
            NavStop();
        }
        else
        {
            NavStart();
        }

        if (!runAway)
        {
            // 타겟 시야 반경 내
            if (TargetIsInSight())
            {
                // 타겟 따라가기
                NavStart();
                SetNav(target.position);
                animator.SetBool("isWalk", true);

                // 공격거리보다 멀면 공격 X
                if (distance > attackRange)
                {
                    enemyCanAttack = false;
                }

                // 공격 거리 이내
                else if (distance < attackRange)
                {
                    LookAtPlayer();

                    animator.SetBool("isRun", false);
                    animator.SetBool("isWalk", false);
                    nav.stoppingDistance = attackRange;

                    if (Time.time > lastShotFired + attackTime)
                    {
                        StartCoroutine(Attack());
                    }
                }
            }

            // 타겟 시야 반경 밖
            // 발견 했을 때 지속 추격
            else if ((playerHasBeenSeen) && (!targetIsOutOfSight))
            {
                animator.SetBool("isWalk", true);
                lostPlayerTimer = Time.time + huntingTimer;

                StartCoroutine(HuntDownTarget());
            }

            // 발견 못했거나 moveableRadius가 0 또는 플레이어와의 거리가 moveableRadius보다 작으면
            else if (((!playerHasBeenSeen)) && ((moveableRadius == 0) || (distance < moveableRadius)))
            {
                WalkNewPath();
            }

            else if ((!playerHasBeenSeen) && (distance > moveableRadius))
            {
                //animator.SetBool("isWalk", false);
                animator.SetBool("isRun", false);

                if (useWaypoint)
                {
                    Patrol();
                }
                else
                {
                    SetNav(startPos);

                    if (nav.remainingDistance <= nav.stoppingDistance)
                    {
                        NavStop();
                    }
                }
            }
        }

        else if (runAway)
        {
            NavStop();

            if (distance < runAwayDistance)
            {
                enemyCanAttack = false;
                nav.speed = runSpeed;
            }
            else if (distance > runAwayDistance)
            {
                enemyCanAttack = false;
                nav.speed = walkSpeed;
            }

            RunAway();
        }

        // 체력 조건 사용
        if (((myStats.currentHealth) / (myStats.maxHealth) <= 0.3f) && (runAwayByHp))
        {
            enemyCanAttack = false;
            runAway = true;
        }
    }

    private void LookAtPlayer()
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
    }

    IEnumerator Attack()
    {
        if (target != null)
        {
            enemyCanAttack = true;

            if (!enemyIsAttacking)
            {
                enemyIsAttacking = true;

                while (enemyIsAttacking)
                {
                    lastShotFired = Time.time;

                    animator.SetTrigger("attack");

                    yield return new WaitForSeconds(attackTime);

                    enemyIsAttacking = false;
                }
            }
        }
    }

    // 시야 반경 내에 있는지 확인
    private bool TargetIsInSight()
    {
        if (target == null)
        {
            return false;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if ((moveableRadius > 0) && (distance > moveableRadius))
        {
            return false;
        }

        targetOn = true;

        if ((visualRadius > 0) && (distance > visualRadius))
        {
            targetOn = false;
            return false;
        }

        if ((targetOn) && (distance < visualRadius))
        {
            LookAtPlayer();
        }

        RaycastHit sight;

        if (Physics.Linecast(transform.position, target.transform.position, out sight))
        {
            if (!playerHasBeenSeen && sight.transform == target)
            {
                //Debug.Log("Sight");
                playerHasBeenSeen = true;
            }
            return sight.transform == target;
        }
        else
        {
            return false;
        }
    }

    // 타겟 추적 지속
    IEnumerator HuntDownTarget()
    {

        targetIsOutOfSight = true;

        while (targetIsOutOfSight)
        {
            //Debug.Log("Hunt");

            NavStart();
            if (target != null)
            {
                SetNav(target.position);
            }

            if (TargetIsInSight())
            {
                targetIsOutOfSight = false;

                break;
            }

            if (Time.time > lostPlayerTimer)
            {
                targetIsOutOfSight = false;
                playerHasBeenSeen = false;

                break;
            }

            yield return null;
        }
    }

    // 순찰
    private void Patrol()
    {
        animator.SetBool("isRun", false);
        animator.SetBool("isWalk", true);

        if (pauseWaypointControl)
        {
            return;
        }

        Vector3 destination = CurrentPath();


        float distance = Vector3.Distance(transform.position, destination);

        if (distance <= 1.5f)
        {
            if (!pauseWaypointControl)
            {
                pauseWaypointControl = true;
                StartCoroutine(WaypointPause());
            }
            else
            {
                PatrolNewPath();
            }
        }

    }

    IEnumerator WaypointPause()
    {
        yield return new WaitForSeconds(Random.Range(pauseMin, pauseMax));
        PatrolNewPath();
        pauseWaypointControl = false;
    }

    private Vector3 CurrentPath()
    {
        return waypoints[waypointPatrol].position;
    }

    private void PatrolNewPath()
    {
        if (!waypointCountdown)
        {
            waypointPatrol++;

            if (waypointPatrol >= waypoints.GetLength(0))
            {
                if (reversePatrol)
                {
                    waypointCountdown = true;
                    waypointPatrol -= 2;
                }
                else
                {
                    waypointPatrol = 0;
                }
            }
        }
        else if (reversePatrol)
        {
            waypointPatrol--;
            if (waypointPatrol < 0)
            {
                waypointCountdown = false;
                waypointPatrol = 1;
            }
        }
    }

    // 새로운 길 찾기
    private void WalkNewPath()
    {
        NavStart();
        if (state == "idle")
        {
            Vector3 randomPos = Random.insideUnitSphere * 10f;
            NavMeshHit navHit;
            NavMesh.SamplePosition(transform.position + randomPos, out navHit, 10f, NavMesh.AllAreas);
            SetNav(navHit.position);
            state = "walk";
        }

        if (state == "walk")
        {
            animator.SetBool("isWalk", true);
            if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
            {
                animator.SetBool("isWalk", false);

                deltaTime += Time.deltaTime;

                if (deltaTime >= delayTime)
                {
                    state = "idle";

                    deltaTime = 0f;
                }
            }
        }
    }

    private void SetNav(Vector3 target)
    {
        nav.SetDestination(target);
    }

    private void NavStart()
    {
        nav.isStopped = false;
    }

    private void NavStop()
    {
        nav.isStopped = true;
    }

    private void RunAway()
    {
        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", true);

        Vector3 runAwayDir = (transform.position - targetPos).normalized;

        runAwayDir.y = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(runAwayDir), Time.deltaTime * rotationSpeed);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        Vector3 direction = runAwayDir * nav.speed * Time.deltaTime;

        this.transform.position += direction;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, runAwayDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, visualRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, moveableRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + this.transform.forward);
    }

    public void AttackHit()
    {
        if (atkMng == null) { Debug.LogError(atkMng); }
        else { atkMng.Attack(); }
    }
}