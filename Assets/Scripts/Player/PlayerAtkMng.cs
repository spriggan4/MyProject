//18년 6월 3일 황재석
//에너미가 사용하는 AtkMng와 별개의 스크립트 이용 필요성을 느껴서 제작

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAtkMng : MonoBehaviour
{
    [SerializeField] private GameObject specialAtkUiText = null;
    private const float TEXT_UI_HEIGHT = 3f;//플레이어와 알림 텍스트의 거리

    private const float COMBO_NUM_MAKING_SPECIAL_ATK = 300f;//콤보는 최대치를 300으로 설정함.

    private Transform objTr = null;
    private Animator playerAnim = null;
    private Weapon equippedWeapon = null;
    private ComboSystemMng comboSystemMng = null;
    private RectTransform specialAtkUiTextRct = null;//조건이 충족되면 화면에 글자를 띄우기 위한 변수
    private Text textUi = null;//조건이 충족되면 화면에 글자를 띄우기 위한 변수

    private bool isEquippedWeapon = false;
    private bool canUseSpecialAtk = false;//update에서 조건들이 충족되면 true 값 들어가고 playerController에서 'E'를 누르면 실행
    private bool isSpecialAtking = false;
    private bool isInTutorial = true;//듀토리얼 상태에선 다르도록 만들기 위해 생성한 변수

    [SerializeField] private float specialAtkMaintableTime = 0f;//스페셜 공격을 시작했을때 유지되는 시간
    private float defaultSpecialAtkMaintableTime = 0f;


    public Weapon EquippedWeapon
    {
        set
        {
            equippedWeapon = value;
        }
    }

    public bool IsEquippedWeapon
    {
        get
        {
            return isEquippedWeapon;
        }
        set
        {
            isEquippedWeapon = value;
        }
    }

    public bool IsSpecialAtking
    {
        get
        {
            return isSpecialAtking;
        }
    }

    public bool IsInTutorial
    {
        get
        {
            return isInTutorial;
        }
    }

    private void Start()
    {
        objTr = this.gameObject.transform;

        playerAnim = this.gameObject.GetComponent<Animator>();
        if (!playerAnim)
        {
            Debug.LogError("PlayerAtkMng의 playerAnim is null");
            Debug.Break();
        }

        comboSystemMng = ComboSystemMng.GetInstance();
        if (!comboSystemMng)
        {
            Debug.LogError("PlayerAtkMng의 comboSystemMng is null");
        }

        defaultSpecialAtkMaintableTime = specialAtkMaintableTime;

        if (!specialAtkUiText)
        {
            Debug.LogError("PlayerAtkMng의 specialAtkUiText is null");
        }
        else
        {
            textUi = specialAtkUiText.GetComponent<Text>();
            if (!textUi)
                Debug.LogError("textUi is Null , Maybe comboTextUi dont have Text Component");
            specialAtkUiTextRct = specialAtkUiText.GetComponent<RectTransform>();
        }
        specialAtkUiText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isEquippedWeapon)
        {
            //조건에 따라 canUseSpecialAtk 변수에 ture 값이 들어가고 알림 텍스트를 띄우는 코드
            if (comboSystemMng.SpecialAtkGageNum >= COMBO_NUM_MAKING_SPECIAL_ATK &&
                !canUseSpecialAtk && !isSpecialAtking)
            {
                canUseSpecialAtk = true;
                Vector3 objPos = this.gameObject.transform.position;
                objPos.y += TEXT_UI_HEIGHT;
                specialAtkUiTextRct.position = Camera.main.WorldToScreenPoint(objPos);
                specialAtkUiText.gameObject.SetActive(true);
            }
            //SpecialAtkGageNum 수치가 0이하로 떨어지면 꺼짐. 수치 조절은 특수공격 실행 후 시작되는 코루틴 타이머로 조절
            else if (comboSystemMng.SpecialAtkGageNum <= 0)
            {
                canUseSpecialAtk = false;
                specialAtkUiText.gameObject.SetActive(false);
            }

            //원거리 공격은 적용되지 않도록 하기 위한 코드
            if (equippedWeapon.isMeleeWeapon)
            {
                if (equippedWeapon.DurabilityCur > 66)
                {
                    equippedWeapon.SlotColor = Color.green;
                }
                else if (equippedWeapon.DurabilityCur > 33)
                {
                    equippedWeapon.SlotColor = Color.yellow;
                }
                else
                {
                    equippedWeapon.SlotColor = Color.red;
                }

                if (equippedWeapon.DurabilityCur <= 0)
                {
                    equippedWeapon.SlotColor = Color.white;
                    isEquippedWeapon = false;
                    canUseSpecialAtk = false;
                    specialAtkUiText.gameObject.SetActive(false);
                    equippedWeapon.DestroyWeapon();
                }
            }
        }
    }

    public void SetForAtk()
    {
        equippedWeapon.SetForWeapon(this.gameObject.transform, isSpecialAtking);
    }

    public void Attack()
    {

        if (isEquippedWeapon)
        {
            equippedWeapon.Attack(playerAnim);
        }

    }

    public void SetSpecialAtk()
    {
        if (isEquippedWeapon)
            if (canUseSpecialAtk && !isSpecialAtking)
            {
                equippedWeapon.SetForSpecialAtk();

                canUseSpecialAtk = false;
                comboSystemMng.ComboTimer = false;
                isSpecialAtking = true;
                specialAtkMaintableTime = defaultSpecialAtkMaintableTime;
                specialAtkUiText.gameObject.SetActive(false);
                //튜토리얼 도중에는 SpecialGageNum가 떨어지지 않도록 만드는 코드
                if (!isInTutorial)
                {
                    StartCoroutine(SpecialAtkMaintableTimeAfterSpecialAtkUse());
                }
            }
    }

    public void EndTutorial()
    {
        isInTutorial = false;
        isSpecialAtking = false;
        equippedWeapon.SetForNormalAtk();
    }

    private void WeaponAttack()
    {
        equippedWeapon.WeaponAttack();
    }

    private IEnumerator SpecialAtkMaintableTimeAfterSpecialAtkUse()
    {
        while (specialAtkMaintableTime > 0)
        {
            specialAtkMaintableTime -= Time.deltaTime;
            yield return null;
        }
        comboSystemMng.ComboTimer = true;
        comboSystemMng.ResetSpecialAtkGageNum();
        isSpecialAtking = false;
        equippedWeapon.SetForNormalAtk();
    }
}