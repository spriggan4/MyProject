using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMng : MonoBehaviour
{
    //public string sceneName;
    private bool isPause = false;
    private static GameMng instance = null;
    private Transform resultUI = null;

    private void Awake()
    {
        resultUI = this.transform.GetComponentInChildren<RectTransform>();
        if (resultUI == null) { return; }
        resultUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetActiveResultCanvas();
        }
    }
    public static GameMng Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(GameMng)) as GameMng;

                if (instance == null)
                {
                    Debug.LogError("GameMng is null");
                    return null;
                }
            }
            return instance;
        }
    }

    public void GameOver()
    {
        StartCoroutine(GameMng.Instance.GameOver(1));
    }

    public void Menu()
    {
        if (!isPause)
            SetOnMenu();
        else
            SetOffMenu();
    }
    public void SetOnMenu()
    {
        Time.timeScale = 0;
        isPause = true;
    }
    public void SetOffMenu()
    {
        if (isPause)
        {
            resultUI.gameObject.SetActive(false);
            Time.timeScale = 1;
            isPause = false;
        }
    }

    public IEnumerator GameOver(int wait)
    {
        yield return new WaitForSeconds(wait);

        //You Die씬 불러오기
        SceneManager.LoadSceneAsync(3);
    }

    public void SetActiveResultCanvas()
    {
        StartCoroutine(GameMng.Instance.SetActiveResultCavasCoroutine());
    }

    private IEnumerator SetActiveResultCavasCoroutine()
    {
        resultUI.gameObject.SetActive(true);
        Menu();
        yield return null;
    }


}
