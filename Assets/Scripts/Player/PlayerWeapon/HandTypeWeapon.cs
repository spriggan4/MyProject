//18년 5월 16일 황재석
//근접공격 무기 스크립트

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTypeWeapon : Weapon
{
    public GameObject weaponMeshObj = null;

    private Transform objTr = null;
    private WeaponMeshCtrl weaponMeshObjCtrl = null;

    public float atkStartDist = 0f; //공격이 시작하는 플레이어와의 거리
    public float attackRange = 0f; //공격 사정거리
    public float weaponAngle = 0f; //공격의 부채꼴 크기
    public int durabilityMax = 100;//무기의 최대 내구도
    public int durabilityReduce = 50;//한번 공격시 떨어지는 내구도 수치

    private int durabilityCur = 0;//현재 내구도 수치를 저장하는 변수

    private float objRotY_AtFirst = 0f;/*Instantiate로 처음 생성할때 플레이어의 회전 Y값을 저장하는 변수
                                       (로컬이기 때문에 0으로 만들어지는 오브젝트의 회전 값을 플레이어와 맞추기 위한 변수)*/

    public override int DurabilityCur
    {
        get
        {
            return durabilityCur;
        }
    }

    private void Start()
    {
        durabilityCur = durabilityMax; ;
    }

    public override void DestoryWeaponMesh()
    {
        this.objTr = null;
        weaponMeshObjCtrl.DestroyWeaponMesh();
    }

    public override void SetForWeapon(Transform _objTr, bool _isSpecialAtking)
    {
        this.objTr = _objTr;
        base.isSpecialAtking = _isSpecialAtking;
        GameObject weaponMesh = Instantiate<GameObject>(weaponMeshObj, this.objTr.position, Quaternion.identity);

        weaponMeshObjCtrl = weaponMesh.gameObject.GetComponent<WeaponMeshCtrl>();
        if (weaponMeshObjCtrl == null)
        {
            Debug.LogError("웨폰메쉬컨트롤 Null");
        }

        if (!base.isSpecialAtking)
        {
            SetForNormalAtk();
        }
        else
        {
            SetForSpecialAtk();
        }

        //Enemy가 무기의 내구도를 깍을 수 있도록 무기의 충돌체에다가 무기의 정보를 넣는 코드
        weaponMeshObjCtrl.WeaponGameObject = this.gameObject.GetComponent<Weapon>();

        //atkSpeed과 높을 수록 어택 이후 재공격 시간이 짧아지도록 만듬.
        //0.8밑으로 내려가면 두 번 공격하는 현상이 생겨서 고정시킴.
        waitingTimeForAtk = 3.0f - attackSpeed;
        if (waitingTimeForAtk <= 0.8f) { waitingTimeForAtk = 0.8f; }
    }

    //웨폰 메쉬 크기를 설정하는 함수
    public override void SetForNormalAtk()
    {
        isSpecialAtking = false;

        objRotY_AtFirst = objTr.transform.rotation.eulerAngles.y;

        weaponMeshObjCtrl.Damage = this.weaponDamage;

        float[] tmpAngle = new float[] { this.objTr.rotation.eulerAngles.y - (weaponAngle / 2),
            this.objTr.rotation.eulerAngles.y + (weaponAngle / 2) };

        weaponMeshObjCtrl.MakeFanShape(tmpAngle, attackRange);

        //설정만 해두고 끈 다음 Attack 함수 실행때만 잠시도록 켜지게 하기 위한 코드
        weaponMeshObjCtrl.gameObject.SetActive(false);
    }

    public override void SetForSpecialAtk()
    {
        isSpecialAtking = true;

        objRotY_AtFirst = objTr.transform.rotation.eulerAngles.y;

        weaponMeshObjCtrl.Damage = this.weaponDamage * 1.5f;

        float[] tmpAngle = new float[] { this.objTr.rotation.eulerAngles.y - (weaponAngle / 2),
            this.objTr.rotation.eulerAngles.y + (weaponAngle / 2) };

        weaponMeshObjCtrl.MakeFanShape(tmpAngle, attackRange * 2);
        weaponMeshObjCtrl.gameObject.SetActive(false);
    }

    public override void Attack(Animator _objAnimator)
    {
        //isAtking은 코루틴에서 시간 계산 후 false값이 들어가도록 설정.
        if (!isAtking)
        {
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
            elapsedTimeAfterAtk = 0.0f;
            isAtking = true;

            //애니 이벤트로 WeaponAttack 실행
            _objAnimator.SetTrigger("Attack");
        }
    }

    public override void WeaponAttack()
    {
        StartCoroutine(MeshActivation());
    }

    public override void StopAtking()
    {
        isAtking = false;
    }

    public override void SubtractDurability()
    {
        if (!isSpecialAtking)
        {
            durabilityCur -= durabilityReduce;
        }
    }

    private IEnumerator MeshActivation()
    {
        while (waitingTimeForAtk > elapsedTimeAfterAtk)
        {
            elapsedTimeAfterAtk += Time.deltaTime;

            //메쉬가 잠시 실행되고 꺼지도록 canAttack을 넣고 리턴은 WaitForFixedUpdate()로 실행
            if (canAttack)
            {
                weaponMeshObjCtrl.transform.position = objTr.position;

                float objRotY = objTr.eulerAngles.y - objRotY_AtFirst;
                weaponMeshObjCtrl.transform.rotation = Quaternion.Euler(0f, objRotY, 0f);

                weaponMeshObjCtrl.gameObject.SetActive(true);
                canAttack = false;
            }
            else
            {
                weaponMeshObjCtrl.gameObject.SetActive(false);
            }
            yield return new WaitForFixedUpdate();
        }
        StopAtking();
    }
}
