﻿//18년 5월 7일
//레벨업 시스템도 넣지 않을까 싶어서 만들었지만 기획 도중 빠져서 사용 안됨.
//나중에라도 사용될까봐 남김

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    [SerializeField] private float normalAtkSpeed = 0f;
    [SerializeField] private float damage = 0f;
    [SerializeField] private float movementSpeed = 0f;
    [SerializeField] private float resistance = 0f;//저항력
    [SerializeField] private float specialAtkMaintableTime = 0f;//스페셜 공격을 시작했을때 유지되는 시간

    public float NormalAtkSpeed
    {
        get
        {
            return normalAtkSpeed;
        }
    }

    public float Damage
    {
        get
        {
            return damage;
        }
    }

    public float MovementSpeed
    {
        get
        {
            return movementSpeed;
        }
    }

    public float Resistance
    {
        get
        {
            return resistance;
        }
    }

    public float SpecialAtkMaintableTime
    {
        get
        {
            return specialAtkMaintableTime;
        }
    }
}
