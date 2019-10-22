using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMng : MonoBehaviour {
    private GameObject effectBloodSpray = null;
    private GameObject effectBulletImpactFleshBig = null;
    private GameObject effectBulletImpactMetal = null;
    private GameObject effectBulletImpactWood = null;
    private GameObject effectPlasmaExp = null;
    private GameObject effectSmallExp = null;

    public GameObject EffectBloodSprray() { return effectBloodSpray; }
    public GameObject EffectBulletImpactFleshBig() { return effectBulletImpactFleshBig; }
    public GameObject EffectBulletImpactMetal() { return effectBulletImpactMetal; }
    public GameObject EffectBulletImpactWood() { return effectBulletImpactWood; }
    public GameObject EffectPlasmaExp() { return effectPlasmaExp; }
    public GameObject EffectSmallExp() { return effectSmallExp; }

    private static ParticleMng instance = null;
    private void Start () {
        effectBloodSpray = Resources.Load("EffectExamples/Blood/Prefabs/BloodSprayEffect") as GameObject;
        effectBulletImpactFleshBig = Resources.Load("EffectExamples/WeaponEffects/Prefabs/BulletImpactFleshBigEffect") as GameObject;
        effectBulletImpactMetal = Resources.Load("EffectExamples/WeaponEffects/Prefabs/BulletImpactMetalEffect") as GameObject;
        effectBulletImpactWood = Resources.Load("EffectExamples/WeaponEffects/Prefabs/BulletImpactWoodEffect") as GameObject;
        effectPlasmaExp = Resources.Load("EffectExamples/FireExplosionEffects/Prefabs/PlasmaExplosionEffect") as GameObject;
        effectSmallExp = Resources.Load("EffectExamples/FireExplosionEffects/Prefabs/SmallExplosionEffect") as GameObject;
    }

    public static ParticleMng GetInstance()
    {
        if (!instance)
        {
            instance = (ParticleMng)GameObject.FindObjectOfType(typeof(ParticleMng));
            if (!instance)
                Debug.LogError("Cant Find GameObject what have ParticleMng Component");
        }
        return instance;
    }
    
}