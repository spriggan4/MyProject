using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour {
    public float heightMax = 0.75f;
    public float heightMin = 0.5f;
    public float defaultSpeed = 0.3f;
    private Transform tr = null;
    private Weapon weapon = null;
    private bool isSpeedDown = false;
    private float speed = 0f;

	private void Start () {
        tr = this.transform;
        weapon = GetComponent<Weapon>();
        if (weapon == null) { weapon = tr.gameObject.AddComponent<Weapon>(); }
        speed = defaultSpeed;
	}
    private void Update()
    {
        if (!weapon.IsPlayerEquipped)
        {
            Vector3 newPos = tr.position;
            if (newPos.y >= heightMax) {
                isSpeedDown = true;
                speed = -defaultSpeed;
            }
            else if (newPos.y <= heightMin) {
                isSpeedDown = false;
                speed = defaultSpeed;
            }
            newPos.y += speed * Time.deltaTime;
            tr.position = newPos;
        }
    }
}
