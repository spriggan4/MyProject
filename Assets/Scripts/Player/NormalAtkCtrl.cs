//18년 5월 16일 황재석
//근접공격 무기 스크립트

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAtkCtrl : MonoBehaviour
{
    public List<string> listSoundName = null;

    public GameObject atkMeshObj = null;

    private Transform objTr = null;
    private WeaponMeshCtrl normalAtkMeshObjCtrl = null;
    private IEnumerator MeshCouroutine=null;

    public float atkStartDist = 0f; //공격이 시작하는 플레이어와의 거리
    public float attackRange = 0f; //공격 사정거리
    public float normalAtkWidth = 0f; //공격의 부채꼴 크기

    private float objRotY_AtFirst = 0f;/*Instantiate로 처음 생성할때 플레이어의 회전 Y값을 저장하는 변수
                                       (로컬이기 때문에 0으로 만들어지는 오브젝트의 회전 값을 플레이어와 맞추기 위한 변수)*/
    private float elapsedTimeAfterAtk=0f;
    private float waitingTimeForAtk = 0f;

    private bool isNormalAtking = false;
    private bool isLeavingMeshElapseTime = false;

    public void SetNormalAtk(float _damage,float _waitingTimeForAtk)
    {
        waitingTimeForAtk = _waitingTimeForAtk;

        objTr = this.transform;
        GameObject atkMesh = Instantiate<GameObject>(atkMeshObj, this.objTr.position, Quaternion.identity);

        normalAtkMeshObjCtrl = atkMesh.gameObject.GetComponent<WeaponMeshCtrl>();
        if (normalAtkMeshObjCtrl == null)
        {
            Debug.LogError("웨폰메쉬컨트롤 Null");
        }

        MeshCouroutine = MeshActivation();

        //아래로는 웨폰 메쉬 크기를 설정하는 코드
        objRotY_AtFirst = objTr.transform.rotation.eulerAngles.y;

        normalAtkMeshObjCtrl.Damage = _damage;

        float[] tmpAngle = new float[] { this.objTr.rotation.eulerAngles.y - (normalAtkWidth / 2),
            this.objTr.rotation.eulerAngles.y + (normalAtkWidth / 2) };

        //끝에 false는 노멀어택이 추가되면서 웨폰인지 노멀인지를 감별하기 위한 변수
        normalAtkMeshObjCtrl.MakeFanShape(tmpAngle, attackRange,false);

        //Attack 함수 실행때만 잠시 켜지도록 off
        normalAtkMeshObjCtrl.gameObject.SetActive(false);
    }

    public void Attack(Animator _objAnimator)
    {
        Debug.Log("isNormalAtking = " + isNormalAtking);
        //Debug.Log("waitingTimeForAtk = " + waitingTimeForAtk);
        //Debug.Log("elapsedTimeAfterAtk = " + elapsedTimeAfterAtk);
        //isAtking은 코루틴에서 시간 계산 후 false값이 들어가도록 설정.
        if (!isNormalAtking)
        {
            isLeavingMeshElapseTime = true;

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

            isNormalAtking = true;

            //애니 이벤트로 NormalAttack 실행
            _objAnimator.SetTrigger("NormalAtk");
        }
    }

    public void NormalAttack()
    {
        StartCoroutine(MeshActivation());
        Debug.Log("NormalAtk 실행");
    }

    public void StopNormalAtking()
    {
        isNormalAtking = false;
    }

    public void StopNormalAtkMeshCouroutine()
    {
        StopCoroutine(MeshCouroutine);
    }

    public void DestoryMesh()
    {
        normalAtkMeshObjCtrl.DestroyWeaponMesh();
    }

    private IEnumerator MeshActivation()
    {
        while (waitingTimeForAtk > elapsedTimeAfterAtk)
        {
            elapsedTimeAfterAtk += Time.deltaTime;

            //메쉬가 잠시 실행되고 꺼지도록 canAttack을 넣고 리턴은 WaitForFixedUpdate()로 실행
            if (isLeavingMeshElapseTime)
            {
                normalAtkMeshObjCtrl.transform.position = objTr.position;

                float objRotY = objTr.eulerAngles.y - objRotY_AtFirst;
                normalAtkMeshObjCtrl.transform.rotation = Quaternion.Euler(0f, objRotY, 0f);

                normalAtkMeshObjCtrl.gameObject.SetActive(true);
                isLeavingMeshElapseTime = false;
            }
            else
            {
                normalAtkMeshObjCtrl.gameObject.SetActive(false);
            }
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("메쉬 while 다음 실행");
        isNormalAtking = false;
    }
}
