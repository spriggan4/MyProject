using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{

    public delegate void AttackHitDelegate();
    public AttackHitDelegate attackHit = null;

    public void AttackHitEvent()
    {
        if (attackHit != null) attackHit();
    }
}