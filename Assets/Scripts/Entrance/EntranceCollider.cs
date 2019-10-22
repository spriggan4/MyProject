//18년 7월 4일 황재석

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceCollider : MonoBehaviour {
    private BoxCollider entranceBoxCollider = null;

    private void Start()
    {
        entranceBoxCollider = this.gameObject.GetComponent<BoxCollider>();
    }

    public void RemoveEntranceBoxColliderTrigger()
    {
        entranceBoxCollider.isTrigger = false;
    }
}
