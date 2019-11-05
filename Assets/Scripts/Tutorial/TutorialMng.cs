//18년 7월 16일 황재석
//튜토리얼 매니져

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMng : MonoBehaviour
{
    public bool TutorialSwitch;

    private TutorialTextMng tutorialTextMng = null;
    private GameObject player = null;
    private PlayerController playerController = null;
    private PlayerAtkMng playerAtkMng = null;
    private ComboSystemMng comboSystemMng = null;

    private EnemyStats[] enemies = null;

    private bool isSetNeedDutorialThings = false;//듀토리얼 상황에 맞춰 함수들을 실행시킨 후 다시 들어가지 않도록 하기 위한 변수
    private bool isEnemiesAllDead = false;//적들 다수를 생성 후 모두 죽었는지 확인하기 위한 변수

    void Start()
    {
        tutorialTextMng = this.gameObject.GetComponentInChildren<TutorialTextMng>();
        if (!tutorialTextMng)
        {
            Debug.LogError("TutorialMng의 dutorialTextMng is null");
            Debug.Break();
        }

        player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
        {
            Debug.LogError("TutorialMng의 player is null");
            Debug.Break();
        }
        else
        {
            playerController = player.gameObject.GetComponent<PlayerController>();
            if (!playerController) { Debug.LogError("DutorialMng의 playerController is null"); }

            playerAtkMng = player.gameObject.GetComponent<PlayerAtkMng>();
            if (!playerAtkMng) { Debug.LogError("DutorialMng의 playerAtkMng is null"); }

            comboSystemMng = player.gameObject.GetComponent<ComboSystemMng>();
            if (!comboSystemMng) { Debug.LogError("DutorialMng의 comboSystemMng is null"); }
        }

        enemies = this.gameObject.GetComponentsInChildren<EnemyStats>();
        for (int i = 0; i < enemies.Length; ++i)
        {
            if (!enemies[i]) { Debug.Log("DutorialMng의 enemies[" + i + "] is null"); }

            else if (enemies[i])
            {
                enemies[i].gameObject.SetActive(false);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TutorialSwitch == true)
        {
            //적들이 모두 죽었는지 체크하고 다음으로 넘기기 위한 코드
            //enemies[0]은 혼자 나와 죽였으므로 1부터 시작.
            for (int i = 1; i < enemies.Length; ++i)
            {
                if (enemies[i].IsDead == false)
                {
                    break;
                }
                else if (i == enemies.Length - 1)
                {
                    isEnemiesAllDead = true;
                }
            }

            //'F'키로 무기를 주을 때 코드
            if (tutorialTextMng.CountSentencesNum() == 11 && !isSetNeedDutorialThings)
            {
                tutorialTextMng.TextButtonOff();
                playerController.IsInputSwitch = true;
                isSetNeedDutorialThings = true;
            }

            //무기 얻은 이후 텍스트 박스와 버튼 살리기
            else if (tutorialTextMng.CountSentencesNum() == 11 && playerAtkMng.IsEquippedWeapon)
            {
                tutorialTextMng.DisplayNextSentence();
                tutorialTextMng.TextButtonOn();
                isSetNeedDutorialThings = false;
            }

            //고블린 한마리가 등장할때 코드, 플레이어가 공격하지 않아 체력
            else if (tutorialTextMng.CountSentencesNum() == 4 && !enemies[0].IsDead
                && !isSetNeedDutorialThings)
            {
                tutorialTextMng.TextButtonOff();
                tutorialTextMng.TextBoxOff();
                enemies[0].gameObject.SetActive(true);
                comboSystemMng.ComboSystemSwitch = true;
                isSetNeedDutorialThings = true;
            }

            //고블린 한마리를 죽인 이후 콤보 시스템을 죽이고 체력을 원상 복구 시킨 후 텍스트 박스와 버튼을 살림.
            else if (tutorialTextMng.CountSentencesNum() == 4 && enemies[0].IsDead
                && isSetNeedDutorialThings)
            {
                tutorialTextMng.TextButtonOn();
                tutorialTextMng.TextBoxOn();
                comboSystemMng.ComboTimer = false;
                comboSystemMng.ComboSystemSwitch = false;
                comboSystemMng.ResetHpToDefault();
                isSetNeedDutorialThings = false;
            }

            //'E'누르라는 문구 나올 때 코드, specialGageNum를 풀로 채운다.
            else if (tutorialTextMng.CountSentencesNum() == 2 && !playerAtkMng.IsSpecialAtking
                && !isSetNeedDutorialThings)
            {
                tutorialTextMng.TextButtonOff();
                comboSystemMng.FillInSpecialGageNum();
                isSetNeedDutorialThings = true;
            }

            //'E'를 눌러서 playerAtkMng.IsSpecialAtking가 true 바뀌면 다음 text를 띄우고 버튼도 살림.
            else if (tutorialTextMng.CountSentencesNum() == 2 && playerAtkMng.IsSpecialAtking
                && isSetNeedDutorialThings)
            {
                tutorialTextMng.DisplayNextSentence();
                tutorialTextMng.TextButtonOn();
                isSetNeedDutorialThings = false;
            }

            //4명의 적을 보이고 텍스트 박스와 버튼 닫기
            else if (tutorialTextMng.CountSentencesNum() == 0 && playerAtkMng.IsSpecialAtking
                && !isEnemiesAllDead && !isSetNeedDutorialThings)
            {
                for (int i = 1; i < enemies.Length; ++i)
                {
                    enemies[i].gameObject.SetActive(true);
                }
                tutorialTextMng.TextButtonOff();
                tutorialTextMng.TextBoxOff();
                isSetNeedDutorialThings = true;
            }

            //마지막 텍스트를 띄운 이후 4초 후 Mng오브젝트 UI오브젝트 모두 파괴.
            else if (tutorialTextMng.CountSentencesNum() == 0 && playerAtkMng.IsSpecialAtking
                && isEnemiesAllDead && isSetNeedDutorialThings)
            {
                tutorialTextMng.TextBoxOn();
                comboSystemMng.ResetSpecialAtkGageNum();
                comboSystemMng.ComboTimer = true;
                comboSystemMng.IsInTutorial = false;
                //comboSystemMng.ResetSpecialAtkGageNum(); 왜 두번을 실행시켰지?
                playerAtkMng.EndTutorial();
                StartCoroutine(DestroyDutorialObj());
            }
        }

        //TutorialSwitch가 off일때 
        else
        {
            tutorialTextMng.TextButtonOff();
            playerController.IsInputSwitch = true;
            comboSystemMng.ComboTimer = true;
            comboSystemMng.IsInTutorial = false;
            //comboSystemMng.ResetSpecialAtkGageNum(); 왜 두번을 실행시켰지?
            playerAtkMng.EndTutorial();
            tutorialTextMng.DestroyDutorialText();
            Destroy(this.gameObject);
        }
    }

    private IEnumerator DestroyDutorialObj()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f);
            tutorialTextMng.DestroyDutorialText();
            Destroy(this.gameObject);
        }
    }
}
