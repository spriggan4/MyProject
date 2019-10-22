//18년 7월 2일
//ProjectileWeaponType와 별개의 스크립트 필요성 느껴 제작

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileWeapon : EnemyWeapon
{
    [SerializeField]
    private GameObject projectile = null;
    private Transform objTr = null;


    public override void Attack(Transform _objTr)
    {
        this.objTr = _objTr;

        Fire();
    }

    private void Fire()
    {
        Vector3 newPos = this.gameObject.transform.position;
        newPos += (objTr.forward * 1.2f);
        newPos.y = 1.0f;

        Instantiate<GameObject>(projectile, newPos, objTr.transform.rotation);
    }
}
