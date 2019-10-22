//18년 6월 30일

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToEnding : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //엔딩씬 로딩
            SceneManager.LoadSceneAsync(4);
        }
    }
}
