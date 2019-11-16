using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This Component For Player
// comboUiBar is only use GUI Image GameObject
public class ComboSystemMng : MonoBehaviour
{
    [SerializeField] private GameObject specialAtkGageBar = null;
    [SerializeField] private GameObject comboUiBar = null;
    public float hp = 0f;

    //색상 변화량, 최대치가 1이기 때문에 파워 최대치 만큼을 나눠서 설정
    //private const float COLOR_STEP = 1f/MAX_GAGE;
    private const float MAX_GAGE = 300.0f;

    private const float textUiHeight = 4f;
    private static ComboSystemMng instance = null;
    private Transform tr = null;
    private RectTransform specialAtkGageBarRT = null;
    private RectTransform comboUiBarRctOver = null;
    private Image specialAtkGageBarImg = null;
    [SerializeField] private float specialAtkGageNum = 0f;
    private float maxHP = 0f;
    private float curHP = 0f;

    [SerializeField] private float hpMaintainableTimeAfterHit = 0f; //공격 이후 콤보를 보존할 수 있는 시간
    private float defaultHpMaintainableTimeAfterHit = 0f;//인스펙터에서 설정한  maintainableTimeAfterHit를 따로 저장하기 위한 변수
    private bool hasHit = false; // 공격하면 True를 넣어서 일정시간동안 체력을 보존시키기
    private bool comboTimer = true;//필살기 공격을 할 때 외부에서 작동 안하도록 만들기 위한 변수
    private bool comboSystemSwitch = false;
    private bool isInTutorial = true;

    public float SpecialAtkGageNum
    {
        get
        {
            return specialAtkGageNum;
        }
    }

    public bool ComboTimer
    {
        set
        {
            comboTimer = value;
        }
    }

    public bool ComboSystemSwitch
    {
        set
        {
            comboSystemSwitch = value;
        }
    }

    public bool IsInTutorial
    {
        set
        {
            isInTutorial = value;
        }
    }

    private void Start()
    {
        tr = this.transform;
        maxHP = hp;
        curHP = hp;
        if (!specialAtkGageBar)
            Debug.LogError("specialAtkGageBar is Null , This var Must have GameObject what TextUI for use ComboSystem");
        if (!comboUiBar)
            Debug.LogError("comboUiBar is NULL , This var Must have GameObject what BarUI for use ComboSystem");
        if (specialAtkGageBar)
        {
            specialAtkGageBarImg = specialAtkGageBar.GetComponent<Image>();
            if (!specialAtkGageBarImg)
                Debug.LogError("specialAtkGageBarImg is Null , Maybe comboTextUi dont have Text Component");
            specialAtkGageBarRT = specialAtkGageBar.GetComponent<RectTransform>();
            comboUiBarRctOver = comboUiBar.GetComponent<RectTransform>();
        }
        //SetTextCount(comboCount);
        StartCoroutine(CoroutineReduceLife());
        defaultHpMaintainableTimeAfterHit = hpMaintainableTimeAfterHit;
    }

    private void Update()
    {
        if (comboTimer)
        {
            if (hasHit)
            {
                if (hpMaintainableTimeAfterHit > 0)
                {
                    hpMaintainableTimeAfterHit -= Time.deltaTime;
                }
                else
                {
                    ResetSpecialAtkGageNum();
                    hasHit = false;
                }
            }
        }
        else
        {
            ResetHpToDefault();
        }

        specialAtkGageBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, specialAtkGageNum);

        //색상변화
        Color newColor = specialAtkGageBarImg.color;
        //green,blue 빼서 red만 남게 만들기
        newColor.g -= 1;
        newColor.b -= 1;
        specialAtkGageBarImg.color = newColor;
    }

    public static ComboSystemMng GetInstance()
    {
        if (!instance)
        {
            instance = (ComboSystemMng)GameObject.FindObjectOfType(typeof(ComboSystemMng));
            if (!instance)
                Debug.LogError("instance is Null,, Can't Find GameObject what Have ComboSystemMng Component");
        }
        return instance;
    }

    public void AddCombo(float num)
    {
        specialAtkGageNum += num;
        if (specialAtkGageNum >= MAX_GAGE)
        {
            specialAtkGageNum = MAX_GAGE;
        }

        ResetHpToDefault();
        hpMaintainableTimeAfterHit = defaultHpMaintainableTimeAfterHit;
        hasHit = true;
    }

    //통로에서 체력을 채워주기 위한 함수
    public void ResetHpToDefault()
    {
        curHP = maxHP;
        Vector2 vecSize = comboUiBarRctOver.sizeDelta;
        vecSize.y = 100f;
        comboUiBarRctOver.sizeDelta = vecSize;
        comboUiBarRctOver.GetComponent<Image>().color = Color.green;
    }

    public void ResetSpecialAtkGageNum()
    {
        specialAtkGageNum = 0;
    }

    //FILL_IN_SPECIAL_GAGE_NUM임.
    public void FillInSpecialGageNum()
    {
        specialAtkGageNum = MAX_GAGE;
    }

    private IEnumerator CoroutineReduceLife()
    {
        float fps60 = 1 / 60f;
        Image imageUiOver = comboUiBarRctOver.GetComponent<Image>();
        if (!imageUiOver)
            Debug.LogError("imageUiOver is NULL,,");
        while (comboUiBar)
        {
            if (comboSystemSwitch)
            {
                Vector2 vecSize = comboUiBarRctOver.sizeDelta;
                Vector3 newPos = tr.position;
                float remainTime = (curHP / maxHP);
               
                if (remainTime <= 0f && !isInTutorial)
                {
                    ResetSpecialAtkGageNum();
                    PlayerController playerCtrl = GetComponent<PlayerController>();
                    if (!playerCtrl)
                        Debug.LogError("playerStats is NULL,,");
                    playerCtrl.Die();
                    Destroy(specialAtkGageBar.gameObject);
                    Destroy(comboUiBar.gameObject);
                }
                else if (isInTutorial)
                {
                    if (remainTime <= 0.1f)
                    {
                        remainTime = 0.1f;
                    }
                }

                curHP -= fps60;

                newPos.y += textUiHeight;
                vecSize.y = remainTime * 100f;
                comboUiBarRctOver.sizeDelta = vecSize;
                if (remainTime > 0.6f)
                    imageUiOver.color = Color.green;
                else if (remainTime > 0.3f)
                    imageUiOver.color = Color.yellow;
                else
                    imageUiOver.color = Color.red;
            }
            yield return new WaitForSeconds(fps60);
        }
    }
}
