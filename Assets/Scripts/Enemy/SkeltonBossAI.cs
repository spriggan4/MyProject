using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeltonBossAI : MonoBehaviour
{
    public bool isDead = false;

    public float moveableRadius = 30.0f;
    public float attackRange = 3.0f;
    public float walkSpeed = 3.0f;
    public float runSpeed = 5.0f;
    public float rotationSpeed = 2.0f;

    private float waitingDelay = 4f;
    private float attackDelay = 4f;
    private int attackCount = 0;

    private Transform target = null;
    private CharacterStat myStats = null;
    private Animator animator = null;
    private NavMeshAgent nav = null;
    private SkeltonBossAttack bossAttack = null;
    
    private Vector3 myPos;
    private Vector3 targetPos;

    private float distance;

    private bossState state = 0;
    private enum bossState { none = 0, waiting, longAtk };

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target == null) { Debug.LogError("target is null"); }

        myStats = this.gameObject.GetComponent<CharacterStat>();
        animator = this.gameObject.GetComponentInChildren<Animator>();
        nav = this.gameObject.GetComponent<NavMeshAgent>();
        bossAttack = this.gameObject.GetComponent<SkeltonBossAttack>();
    }

    void Update()
    {
        if ((!target) || (isDead)) { return; }

        myPos = this.transform.position;
        targetPos = target.position;
        distance = Vector3.Distance(myPos, targetPos);


        if (moveableRadius > distance)
        {
            LookAtPlayer();
            if (state == bossState.none)
            {
                attackDelay -= Time.deltaTime;

                if (attackDelay < 0f)
                {
                    state = bossState.longAtk;
                }
            }

            // 패턴 2, 원거리 공격
            else if (state == bossState.longAtk)
            {
                bossAttack.LongDistanceAttack();

                attackDelay = 4f;

                state = bossState.none;

                ++attackCount;

                if (attackCount == 5)
                {
                    state = bossState.waiting;
                }

            }

            else if (state == bossState.waiting)
            {
                waitingDelay -= Time.deltaTime;
                if(waitingDelay < 0)
                {
                    state = bossState.none;
                    waitingDelay = 4f;
                }
            }
        }
    }

    private void LookAtPlayer()
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
    }
}
