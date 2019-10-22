//18년 6월 30일 황재석

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTutorial : MonoBehaviour {
    private ComboSystemMng comboSystemMng = null;
    private GameObject player = null;
    private PlayerAtkMng playerAtkMng = null;

    // Use this for initialization
    void Start()
    {
        comboSystemMng = ComboSystemMng.GetInstance();
        if (!comboSystemMng)
        {
            Debug.LogError("EndTutorial의 comboSystemMng is null");
            Debug.Break();
        }

        player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
        {
            Debug.LogError("EndTutorial의 playeris null");
            Debug.Break();
        }
        else
        {
            playerAtkMng = player.gameObject.GetComponent<PlayerAtkMng>();
            if (!playerAtkMng)
            {
                Debug.LogError("EndTutorial의 playerAtkMng is null");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            comboSystemMng.ComboSystemSwitch = true;
        }
    }
}
