//18년 7월 13일
//스페셜 어택 공격을 시작하고 끝낼때 플레이어의 외형을 조정하기 위한 매니져

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerModelMng : MonoBehaviour
{
    private WingCtrl wing = null;
    private FireHairCtrl fireHair = null;
    private SparksParticleCtrl sparksParticle = null;

    private NormalHairCtrl normalHair = null;

    // Use this for initialization
    void Start()
    {
        wing = this.gameObject.GetComponentInChildren<WingCtrl>();
        if (!wing)
        {
            Debug.LogError("ChangePlayerModelMng의 wing is Null");
        }

        fireHair = this.gameObject.GetComponentInChildren<FireHairCtrl>();
        if (!fireHair)
        {
            Debug.LogError("ChangePlayerModelMng의 fireHair is Null");
        }

        sparksParticle = this.gameObject.GetComponentInChildren<SparksParticleCtrl>();
        if (!sparksParticle)
        {
            Debug.LogError("ChangePlayerModelMng의 sparksParticle is Null");
        }

        normalHair = this.gameObject.GetComponentInChildren<NormalHairCtrl>();
        if (!normalHair)
        {
            Debug.LogError("ChangePlayerModelMng의 normalHair is Null");
        }

        wing.gameObject.SetActive(false);
        fireHair.gameObject.SetActive(false);
        sparksParticle.gameObject.SetActive(false);
    }

    public void ModelingWithSpecialAtk()
    {
        normalHair.gameObject.SetActive(false);
        wing.gameObject.SetActive(true);
        fireHair.gameObject.SetActive(true);
        sparksParticle.gameObject.SetActive(true);
    }

    public void ModelingWithNormal()
    {
        normalHair.gameObject.SetActive(true);
        wing.gameObject.SetActive(false);
        fireHair.gameObject.SetActive(false);
        sparksParticle.gameObject.SetActive(false);
    }
}
