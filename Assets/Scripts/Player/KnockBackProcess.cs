//18년 5월 14일 황재석

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackProcess : MonoBehaviour
{
    [SerializeField]
    private float knockBackTime = 0f;
    [SerializeField]
    private float forcePow = 0f;

    //넉백 동안 PlayerController의 isInputSwitchOn을 끄기 위해 값을 받음
    private PlayerController playerCtrl = null;
    private Vector3 direction = Vector3.zero;
    private Transform tr = null;
    private Animator animator = null;

    private bool isKnockBackOn = false;
    //넉백 시작 후 체크를 위한 변수
    private float elapsedTimeAfterKnockBack = 0f;

    private void Start()
    {
        tr = this.gameObject.transform;
        playerCtrl = this.gameObject.GetComponent<PlayerController>();
        if (playerCtrl==null)
        {
            Debug.LogError("넉백 시스템의 플레이어 컨트롤러 Null");
            Debug.Break();
        }
        elapsedTimeAfterKnockBack = knockBackTime;
        animator = playerCtrl.Anim;
        if (animator == null)
        {
            Debug.LogError("KnockBackProcess의 animator is Null");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WeaponMesh" && !isKnockBackOn)
        {
            direction = other.transform.forward;
            elapsedTimeAfterKnockBack = 0;
            isKnockBackOn = true;
            playerCtrl.CanInputSwitch = false;
            animator.SetTrigger("TakeDamage");
            StartCoroutine(KnockBack());
        }
    }

    private IEnumerator KnockBack()
    {
        Vector3 newPos = tr.position;
        Quaternion quater = tr.rotation;
        newPos.y += 1f;
        Instantiate(ParticleMng.GetInstance().EffectBloodSprray(), newPos, quater);
        Instantiate(ParticleMng.GetInstance().EffectBulletImpactFleshBig(), newPos, quater);

        while (elapsedTimeAfterKnockBack < knockBackTime)
        {
            direction.y = 0;
            playerCtrl.transform.position += (direction * forcePow) / 100;
            elapsedTimeAfterKnockBack += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        isKnockBackOn = false;
        playerCtrl.CanInputSwitch = true;
    }
}
