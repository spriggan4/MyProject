//18년 6월 28일 황재석

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileWeapon : EnemyWeapon
{
    [SerializeField]
    private GameObject projectile = null;
    private Transform tr = null;

    private float waitingTimeForFire = 0f;


    public override void Attack(Transform _objTr)
    {
        this.tr = _objTr;

        //코루틴 체크에 필요한 시간 초기화
        waitingTimeForFire = 0.2f;
        StartCoroutine(Fire());
    }

    private IEnumerator Fire()
    {
        while (waitingTimeForFire > 0)
        {
            waitingTimeForFire -= Time.deltaTime;
            Vector3 newPos = this.transform.position;
            //적앞에서 발사되도록 방향값으로 거리를 줬음.
            //공중에 떠야 하므로 y값 설정
            newPos += (tr.forward * 1.2f);
            newPos.y = 1.0f;

            //약간씩 각도를 변환시켜 5개의 총알이 부채꼴로 발사되는 코드.
            float projectileAngleX = tr.transform.rotation.eulerAngles.x;
            float projectileAngleY = tr.transform.rotation.eulerAngles.y;
            float projectileAngleZ = tr.transform.rotation.eulerAngles.z;

            Quaternion hopedForProjectileAngle1 = Quaternion.Euler(projectileAngleX, projectileAngleY - 40.0f, projectileAngleZ);
            Quaternion hopedForProjectileAngle2 = Quaternion.Euler(projectileAngleX, projectileAngleY - 20.0f, projectileAngleZ);
            Quaternion hopedForProjectileAngle3 = Quaternion.Euler(projectileAngleX, projectileAngleY, projectileAngleZ);
            Quaternion hopedForProjectileAngle4 = Quaternion.Euler(projectileAngleX, projectileAngleY + 20.0f, projectileAngleZ);
            Quaternion hopedForProjectileAngle5 = Quaternion.Euler(projectileAngleX, projectileAngleY + 40.0f, projectileAngleZ);

            GameObject objProjectile1 = Instantiate<GameObject>(projectile, newPos, hopedForProjectileAngle1);
            GameObject objProjectile2 = Instantiate<GameObject>(projectile, newPos, hopedForProjectileAngle2);
            GameObject objProjectile3 = Instantiate<GameObject>(projectile, newPos, hopedForProjectileAngle3);
            GameObject objProjectile4 = Instantiate<GameObject>(projectile, newPos, hopedForProjectileAngle4);
            GameObject objProjectile5 = Instantiate<GameObject>(projectile, newPos, hopedForProjectileAngle5);

            yield return new WaitForSeconds(0.1f);

        }
    }
}
