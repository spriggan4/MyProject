//18년 7월 15일 황재석
//처음 문을 통과하면 콤보시스템이 켜지면서 본게임이 시작되게 하기 위한 스크립트

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystemOn : MonoBehaviour
{
    ComboSystemMng comboSystemMng = null;

    // Use this for initialization
    void Start()
    {
        comboSystemMng = ComboSystemMng.GetInstance();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            comboSystemMng.ComboSystemSwitch = true;
        }
    }
}
