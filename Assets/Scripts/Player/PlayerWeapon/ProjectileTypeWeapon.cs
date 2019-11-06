//18년 5월 15일 황재석

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTypeWeapon : Weapon
{
    [SerializeField] private GameObject projectile = null;
    [SerializeField] private GameObject specialAtkProjectile = null;
    private Transform objTr = null;

    [SerializeField]
    private int usableCount = 1;

    public override void SetForWeapon(Transform _objTr, bool _isSpecialAtking)
    {
        this.objTr = _objTr;
        base.isSpecialAtking = _isSpecialAtking;

        //atkSpeed과 높을 수록 어택 이후 재공격 시간이 짧아지도록 만듬.
        //0.8밑으로 내려가면 두 번 공격하는 현상이 생겨서 고정시킴.
        waitingTimeForAtk = 3.0f - attackSpeed;
        if (waitingTimeForAtk <= 0.8f) { waitingTimeForAtk = 0.8f; }
    }

    public override void SetForNormalAtk()
    {
        base.isSpecialAtking = false;
    }

    public override void SetForSpecialAtk()
    {
        base.isSpecialAtking = true;
    }

    public override void Attack(Animator _objAnimator)
    {
        //isAtkTimerOn은 코루틴에서 시간 계산 후 false값이 들어가도록 설정.
        if (!isAtking)
        {
            isAtking = true;
            canAttack = true;

            switch (Random.Range(0, 2))
            {
                case 0:
                    AudioMng.GetInstance().PlaySound("AttackSound_1", objTr.position, 100f);
                    break;
                case 1:
                    AudioMng.GetInstance().PlaySound("AttackSound_2", objTr.position, 100f);
                    break;
                case 2:
                    AudioMng.GetInstance().PlaySound("AttackSound_3", objTr.position, 100f);
                    break;
            }
            elapsedTimeAfterAtk = 0f;
            //애니 이벤트로 WeaponAttack 실행
            _objAnimator.SetTrigger("CrossbowAttack");
        }
    }

    public override void WeaponAttack()
    {
        StartCoroutine(Fire(weaponDamage));
    }

    public override void StopAtking()
    {
        isAtking = false;
    }

    private IEnumerator Fire(float _weaponDamage)
    {
        while (waitingTimeForAtk > elapsedTimeAfterAtk)
        {
            elapsedTimeAfterAtk += Time.deltaTime;

            if (canAttack)
            {
                Vector3 newPos = this.transform.position;
                newPos += (this.objTr.forward * 1.2f);
                newPos.y = 1.2f;

                if (!isSpecialAtking)
                {
                    GameObject projectileObj = Instantiate<GameObject>(projectile, newPos, this.objTr.transform.rotation);
                    if (!projectileObj)
                    {
                        Debug.LogError("ProjectileTypeWeapon에서 projectileObj is Null");
                    }

                    NeedWeaponThingsForSystem needWeaponThingsForSystem = projectileObj.gameObject.GetComponent<NeedWeaponThingsForSystem>();
                    needWeaponThingsForSystem.Damage = _weaponDamage;
                    needWeaponThingsForSystem.WeaponGameObject = this.gameObject.GetComponent<Weapon>();

                    SubtractUsableCount();
                }
                else
                {
                    GameObject specialAtkProjectileObj = Instantiate<GameObject>(specialAtkProjectile, newPos, this.objTr.transform.rotation);
                    if (!specialAtkProjectileObj)
                    {
                        Debug.LogError("ProjectileTypeWeapon에서 specialAtkProjectileObj is Null");
                    }

                    NeedWeaponThingsForSystem needWeaponThingsForSystem = specialAtkProjectileObj.gameObject.GetComponent<NeedWeaponThingsForSystem>();
                    needWeaponThingsForSystem.Damage = _weaponDamage * 1.5f;
                    needWeaponThingsForSystem.WeaponGameObject = this.gameObject.GetComponent<Weapon>();
                }

                canAttack = false;
            }
            yield return new WaitForFixedUpdate();
        }
        StopAtking();
    }

    private void SubtractUsableCount()
    {
        usableCount -= 1;
        if (usableCount <= 0) { DestroyWeapon(); }
    }
}
