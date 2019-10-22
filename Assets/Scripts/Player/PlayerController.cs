//18년 5월 8일 황재석

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private Vector3 movement = Vector3.zero; //GetAxisRaw 처리 다음에 반환값을 담을 변수                 
    private Animator playerAnim = null;
    private PlayerStats playerStats = null;
    private PlayerAtkMng playerAtkMng = null;
    private ChangePlayerModelMng changePlayerModelMng = null;

    private bool isAlive = true;
    private bool isInputSwitch = false;
    private bool isInPassage = false;//통로 안에 있는지를 체크해서 카메라가 정면으로 응시하게 만들 변수
    private bool canChangeCamSettingWithDefault = false;//통로를 통과하면 카메라가 탑뷰로 바뀌게 만들 변수
    private bool hasChangedSpecialAtkModel = false;


    private int floorMask; //raycast Layer 정보를 담을 변수                    
    private float camRayLength = 100f; //raycast 거리 값

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
        set
        {
            isAlive = value;
        }
    }

    public bool IsInputSwitch
    {
        get
        {
            return isInputSwitch;
        }
        set
        {
            isInputSwitch = value;
        }
    }

    public bool IsInPassage
    {
        get
        {
            return isInPassage;
        }
    }

    public bool CanChangeCamSettingWithDefault
    {
        get
        {
            return canChangeCamSettingWithDefault;
        }
        set
        {
            canChangeCamSettingWithDefault = value;
        }
    }

    public Animator Anim
    {
        get
        {
            return playerAnim;
        }
    }

    private void Awake()
    {
        playerAnim = this.gameObject.GetComponent<Animator>();
        if (!playerAnim)
        {
            Debug.LogError("플레이어 컨트롤러의 플레이어 애니메이터 Null");
        }

        playerStats = this.gameObject.GetComponent<PlayerStats>();
        if (!playerStats)
        {
            Debug.LogError("플레이어 컨트롤러의 플레이어 스탯 Null");
        }

        playerAtkMng = this.gameObject.GetComponent<PlayerAtkMng>();
        if (!playerAtkMng)
        {
            Debug.LogError("플레이어 컨트롤러의 플레이어 어택 매니져 Null");
        }

        floorMask = LayerMask.GetMask("Floor");

        changePlayerModelMng = this.gameObject.GetComponent<ChangePlayerModelMng>();
        if (!changePlayerModelMng)
        {
            Debug.LogError("플레이어 컨트롤러의 changePlayerModelMng is Null");
        }
    }
    private void Update()
    {
        if (playerAtkMng.IsSpecialAtking && !hasChangedSpecialAtkModel)
        {
            changePlayerModelMng.ModelingWithSpecialAtk();
            hasChangedSpecialAtkModel = true;
        }
        else if (!playerAtkMng.IsSpecialAtking && hasChangedSpecialAtkModel)
        {
            changePlayerModelMng.ModelingWithNormal();
            hasChangedSpecialAtkModel = false;
        }
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerAtkMng.SetSpecialAtk();
            }

            if (isInputSwitch)
            {
                AtkCtrl();

                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");

                Move(h, v, playerStats.MovementSpeed);
                Turning();
                Animating(h, v);
            }

        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        Vector3 newPos = this.transform.position;
        Quaternion quater = this.transform.rotation;
        newPos.y += 1f;

        Instantiate(ParticleMng.GetInstance().EffectSmallExp(), newPos, quater);
        AudioMng.GetInstance().PlaySound("PlayerDie", newPos, 100f);


        Destroy(gameObject);
        GameMng.Instance.GameOver();
    }

    private void AtkCtrl()
    {
        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                playerAtkMng.Attack();
            }
        }
    }

    private void Move(float h, float v, float speed)
    {
        //카메라의 방향값에 h와 v를 곱해 newPos에 넣고 플레이어에 반환함.
        Camera main = Camera.main;
        Vector3 newPos = this.gameObject.transform.position;
        newPos += ((main.transform.forward * v) + (main.transform.right * h)) * speed * Time.deltaTime;
        newPos.y = this.gameObject.transform.position.y;
        this.gameObject.transform.position = newPos;
    }

    private void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - this.gameObject.transform.position;

            //Y는 변함 없으므로 0 처리
            playerToMouse.y = 0f;

            //플레이어 위치에서 마우스 위치로의 방향 값 구하고 저장
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            //저장한 방향 값으로 회전
            this.gameObject.transform.rotation = newRotation;
        }
    }

    private void Animating(float h, float v)
    {
        //방향키 있으면 ||연산자에 의해 1이 반환되어서 true, 방향키 없으면 false 반환 됨.
        bool walking = h != 0f || v != 0f;

        playerAnim.SetBool("IsWalking", walking);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "PassageTriggerCollider")
        {
            isInPassage = true;
        }
        if (other.tag == "ChagingAreaWithDefaultCamSetting")
        {
            canChangeCamSettingWithDefault = true;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PassageTriggerCollider")
        {
            isInPassage = false;
        }
    }
}
