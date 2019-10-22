//18년 5월 12일 황재석

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkMng : MonoBehaviour
{
    private EnemyWeapon enemyWeapon = null;
    private float atkPower = 0f;
    private float atkSpeed = 0f;
    private bool isEquippedWeapon = false;

    //Equip과 동시에 장착할 수 있도록 프로퍼티들 설정
    public EnemyWeapon EnemyWeapon
    {
        set
        {
            enemyWeapon = value;
        }
    }

    public float AtkPower
    {
        get
        {
            return atkPower;
        }
        set
        {
            atkPower = value;
        }
    }


    public float AtkSpeed
    {
        get
        {
            return atkSpeed;
        }
        set
        {
            atkSpeed = value;
        }
    }

    public bool IsEquippedWeapon
    {
        get
        {
            return isEquippedWeapon;
        }
        set
        {
            isEquippedWeapon = value;
        }
    }

    public void Attack()
    {
        //Equip에서 isEquippedWeapon에 값 넣은 후 실행 가능.
        if (isEquippedWeapon)
        {
            enemyWeapon.Attack(this.gameObject.transform);
        }
    }
}
