//18년 7월 9일
//에너미와의 상호 작용에 필요한 요소들이 있음.
//데미지라던가 무기의 정보
//이를 통해 사운드와 데미지 처리 시행.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedWeaponThingsForSystem : MonoBehaviour {

    private Weapon weaponGameObject;

    public Weapon WeaponGameObject
    {
        get { return weaponGameObject; }
        set { weaponGameObject = value; }
    }

    private float damage = 0f;

    public float Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }
}
