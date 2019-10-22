using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDoorCtrl : MonoBehaviour {
    public GameObject obj= null;
    public Animation[] ani = null;

	private void Update()
    {
        if (!obj)
        {
            ani[0].Play("Door_Left");
            ani[1].Play("Door_Right");
            Destroy(this.gameObject);
        }
    }
}
