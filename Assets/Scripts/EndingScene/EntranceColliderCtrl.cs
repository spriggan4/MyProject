//18년 6월 30일 황재석

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceColliderCtrl : MonoBehaviour
{
    private EntranceCollider entranceCollider = null;

    // Use this for initialization
    void Start()
    {
        entranceCollider = this.gameObject.GetComponentInChildren<EntranceCollider>();
        if (!entranceCollider)
        {
            Debug.LogError("입구 콜라이더 컨트롤러의 entranceCollider = Null");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            //통로에 설치된 콜라이더를 플레이어가 건드리면 입구 콜라이더의 트리거가 꺼지면서 다시 못돌아가게 만듬.
            entranceCollider.RemoveEntranceBoxColliderTrigger();

            //통로에 머무리는 동안 계속 체력이 차도록
            ComboSystemMng comboSystemMng = other.gameObject.GetComponent<ComboSystemMng>();
            comboSystemMng.ResetHpToDefault();
        }
    }
}
