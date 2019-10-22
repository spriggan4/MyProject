//18년 7월 4일 황재석

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitColliderCtrl : MonoBehaviour
{
    private BoxCollider exitBoxCollider;

    // Use this for initialization
    void Start()
    {
        exitBoxCollider = this.gameObject.GetComponent<BoxCollider>();
    }

    public void RemoveExitBoxColliderTrigger()
    {
        exitBoxCollider.isTrigger = false;
    }
}