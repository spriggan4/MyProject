//18년 7월 17일 황재석
//제일 처음 출입문이 처음에는 콜라이더 상태였다가 튜토리얼 끝나면 트리거로 바뀌기 위한 스크립트

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColliderWhenEndTutorial : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerAtkMng player = collision.gameObject.GetComponent<PlayerAtkMng>();
            if (!player)
            {
                Debug.LogError("ChangeColliderWhenEndTutorial의 player is null");
                Debug.Break();
            }

            else if (!player.IsInTutorial)
            {
                BoxCollider collider = this.gameObject.GetComponent<BoxCollider>();
                if (!collider)
                {
                    Debug.LogError("ChangeColliderWhenEndTutorial의 collider is null");
                }
                else
                {
                    collider.isTrigger = true;
                }
            }
        }
    }
}
