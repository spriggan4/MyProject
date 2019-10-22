using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasControl : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    public void Resume()
    {
        GameMng.Instance.SetOffMenu();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
