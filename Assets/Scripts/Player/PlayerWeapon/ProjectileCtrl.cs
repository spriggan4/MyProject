//18년 5월 20일 황재석

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCtrl : NeedWeaponThingsForSystem
{
    [Range(10, 400)]
    [SerializeField]
    private float projectileSpeed = 0f;
    private Vector3 direction = Vector3.zero;

    private void Start()
    {
        direction = this.gameObject.transform.forward;
        this.gameObject.GetComponent<Rigidbody>().AddForce(direction * (projectileSpeed * 10));
    }

    private void OnTriggerEnter(Collider other)
    {
        //맵에 깔린 웨폰이나 적이 쏜 발사체와 부딫혀도 문제 없도록 if 처리
        if (other.tag != "Weapon" && other.tag != "WeaponMesh")
        {
            Destroy(this.gameObject);
        }
    }
}
