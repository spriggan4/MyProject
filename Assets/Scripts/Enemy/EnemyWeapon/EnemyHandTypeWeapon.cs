//18년 5월 29일 황재석

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandTypeWeapon : EnemyWeapon
{
    public float atkStartDist = 0.0f;
    public float attackRange = 0.0f;
    public float weaponAngle = 0.0f;

    private EnemyWeaponMeshCtrl enemyWeaponMeshCtrl = null;

    private Transform objTr = null;

    private float waitingTimeForAtk = 0.0f;
    private float elapsedTimeAfterAtk = 0.0f;
    private bool attackSwitchOn = false;
    private bool isReady = false;

    private void Start()
    {
        enemyWeaponMeshCtrl = this.gameObject.GetComponentInChildren<EnemyWeaponMeshCtrl>();
        if (!enemyWeaponMeshCtrl)
        {
            Debug.LogError("에너미핸드타입웨폰에 웨폰메쉬 Null");
        }

        waitingTimeForAtk = 3.0f - attackSpeed;
    }

    public override void Attack(Transform _objTr)
    {
        this.objTr = _objTr;
        elapsedTimeAfterAtk = 0.0f;
        attackSwitchOn = true;
        if (!isReady)
        {
            //콜리전에 사용할 Mesh를 만든다.
            enemyWeaponMeshCtrl.gameObject.SetActive(false);
            float[] tmpAngle = new float[] { this.objTr.rotation.y - (weaponAngle / 2), this.objTr.rotation.y + (weaponAngle / 2) };
            enemyWeaponMeshCtrl.MakeFanShape(tmpAngle, attackRange);

            isReady = true;
        }
        StartCoroutine(MakeTransformMesh());
    }

    private IEnumerator MakeTransformMesh()
    {
        while (waitingTimeForAtk > elapsedTimeAfterAtk)
        {
            //timeForCheck는 어택 실행 후 한번 초기화 된다.
            elapsedTimeAfterAtk += Time.deltaTime;
            if (attackSwitchOn)
            {
                enemyWeaponMeshCtrl.gameObject.SetActive(true);
                attackSwitchOn = false;
            }
            else
            {
                enemyWeaponMeshCtrl.gameObject.SetActive(false);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
