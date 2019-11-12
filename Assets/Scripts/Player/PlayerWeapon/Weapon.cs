using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public float remainTime = 5f;
    public float weaponDamage = 0;
    public float attackSpeed = 0.0f;

    public Color SlotColor = Color.green;
    public List<string> listSoundName = null;

    public bool isMeleeWeapon = true;

    protected float waitingTimeForAtk = 0f;//공격을 위해 기다리는 시간, 공속이랑 연관 있음.
    protected float elapsedTimeAfterAtk = 0f;//공격 후 얼마나 시간이 지났는지 체크하는 변수
    protected bool isAtking = false;
    protected bool canAttack = false;
    protected bool isSpecialAtking = false;

    public virtual void DestoryEverythingForWeaponAtk() { }
    public virtual void SetForWeapon(Transform _objTr, bool _isSpecialAtking) { }
    public virtual void SetForNormalAtk() { }
    public virtual void SetForSpecialAtk() { }
    public virtual void Attack(Animator _objAnimator) { }
    public virtual void WeaponAttack() { }
    public virtual void SubtractDurability(){ }
    public virtual int DurabilityCur { get; set; }
    public virtual float WeaponAngle { get; set; }
    public virtual float AtkRangeDist { get; set; }
    public virtual float AtkStartDist { get; set; }

    public bool IsPlayerEquipped { get; set; }
    public bool IsChangedSlotColor { get; set; }

    public void OnStartRemainTime(float time)
    {
        StartCoroutine(ExpiredRemainTime());
    }

    public void DestroyWeapon()
    {
        IsDestroyed = true;
        //DestroyItem();
    }

    private IEnumerator ExpiredRemainTime()
    {
        DestroyWeapon();
        yield return null;
    }
}
