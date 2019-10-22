using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeltonBossAttack : MonoBehaviour
{
    private AtkMng atkMng = null;
    private Animator ani = null;

    private void Start()
    {
        atkMng = this.gameObject.GetComponent<AtkMng>();
        ani = this.gameObject.GetComponentInChildren<Animator>();

        this.gameObject.GetComponentInChildren<AnimationEventReceiver>().attackHit = AttackHit;
    }

    public void LongDistanceAttack()
    {
        ani.SetTrigger("attack");
    }

    public void AttackHit()
    {
        if (atkMng == null) { Debug.LogError(atkMng); }
        else { atkMng.Attack(); }
    }
}
