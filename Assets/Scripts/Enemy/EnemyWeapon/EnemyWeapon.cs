//18년 7월 9일 황재석

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : Item {

    public float remainTime = 5f;
    public float damage = 0;
    public float attackSpeed = 0.0f;
    public int durabilityMax = 100;
    public int durabilityCur = 100;
    public int usableCount = 5;
    public int durabilityReduce = 50;
    public bool isWeaponTypeMelee = true;
    public Color SlotColor = Color.green;
    public List<string> listSoundName = null;

    private bool isChangedSlotColor = false;

    public virtual void Attack(Transform _objTr) { }
    public virtual float WeaponAngle { get; set; }
    public virtual float AtkRangeDist { get; set; }
    public virtual float AtkStartDist { get; set; }

    public bool IsPlayerEquipped { get; set; }
    public bool IsChangedSlotColor { get; set; }

    public void SubtractDurability(int amount)
    {
        durabilityCur -= amount;
        if (durabilityCur > 66)
        {
            SlotColor = Color.green;
            isChangedSlotColor = true;
        }
        else if (durabilityCur > 33)
        {
            SlotColor = Color.yellow;
            isChangedSlotColor = true;
        }
        else
        {
            SlotColor = Color.red;
            isChangedSlotColor = true;
        }

        if (durabilityCur <= 0)
        {
            SlotColor = Color.white;
            isChangedSlotColor = true;
            DestroyWeapon();
        }
    }

    public void SubtractUsableCount(int count)
    {
        usableCount -= count;
        if (usableCount <= 0) { DestroyWeapon(); }
    }

    public void OnStartRemainTime(float time)
    {
        StartCoroutine(ExpiredRemainTime());
    }

    private IEnumerator ExpiredRemainTime()
    {
        DestroyWeapon();
        yield return null;
    }
    private void DestroyWeapon()
    {
        IsDestroyed = true;
    }
}
