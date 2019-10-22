//18년 6월 19일

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileCtrl : MonoBehaviour
{

    [Range(10,200)]
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
        //맵에 떠 있는 웨폰과 에너미 그리고 발사체끼리 부딫혀 사라지지 않도록 if문 처리
        if (other.tag != "Weapon" && other.tag != "Enemy" && other.tag != "WeaponMesh")
        {
            Destroy(this.gameObject);
        }
    }
}
