using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakingDamage : MonoBehaviour
{
    //인스펙터로 쉬운 조작을 위해 설정
    [SerializeField] private string opponentObjAtkTagName = null;
    private static bool isAttacked = false;
    private bool isDead = false;

    private Transform tr = null;

    public bool IsDead
    {
        set
        {
            isDead = value;
        }
    }

    private void Start()
    {
        tr = this.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDead)
        {
            if (opponentObjAtkTagName == null)
            { Debug.LogError("WeaponTag Name is null"); }

            if (other.tag == opponentObjAtkTagName)
            {
                float weaponDamage = other.gameObject.GetComponent<NeedWeaponThingsForSystem>().Damage;

                CharacterStat objStat = this.gameObject.GetComponent<CharacterStat>();
                WeaponMeshCtrl meshCtrl = other.GetComponent<WeaponMeshCtrl>();

                //Weapon의 메쉬일 경우
                if (meshCtrl.IsWeaponMesh)
                {
                    Weapon weapon = null;

                    if (meshCtrl)
                    {
                        weapon = meshCtrl.WeaponGameObject;
                    }
                    else
                    {
                        weapon = other.GetComponent<ProjectileCtrl>().WeaponGameObject;
                    }

                    Vector3 newPos = tr.position;
                    newPos.y += 1f;

                    // Paticle
                    Instantiate(ParticleMng.GetInstance().EffectBulletImpactWood(), newPos, tr.rotation);
                    Instantiate(ParticleMng.GetInstance().EffectBulletImpactMetal(), newPos, tr.rotation);

                    if (weapon.listSoundName.Capacity > 0)
                    {
                        int rand = Random.Range(0, weapon.listSoundName.Count);
                        AudioMng.GetInstance().PlaySound(weapon.listSoundName[rand], this.transform.position, 120f);
                    }

                    if (!isAttacked && meshCtrl)
                    {
                        isAttacked = true;
                        weapon.SubtractDurability();
                    }

                    // Combo
                    ComboSystemMng.GetInstance().AddCombo(50f);

                    // Taking Damage
                    objStat.TakeDamage(weaponDamage);
                }

                //노멀 어택의 메쉬인 경우
                else
                {
                    NormalAtkCtrl normalAtkCtrl = other.GetComponent<NormalAtkCtrl>();

                    Vector3 newPos = tr.position;
                    newPos.y += 1f;

                    // Paticle
                    Instantiate(ParticleMng.GetInstance().EffectBulletImpactWood(), newPos, tr.rotation);
                    Instantiate(ParticleMng.GetInstance().EffectBulletImpactMetal(), newPos, tr.rotation);

                    //경훈이가 만들었는데 사운드 부분이여서 일단 자름
                    //if (normalAtkCtrl.listSoundName.Capacity > 0)
                    //{
                    //    int rand = Random.Range(0, normalAtkCtrl.listSoundName.Count);
                    //    AudioMng.GetInstance().PlaySound(normalAtkCtrl.listSoundName[rand], this.transform.position, 120f);
                    //}

                    if (!isAttacked)
                    {
                        isAttacked = true;
                    }

                    // Combo
                    ComboSystemMng.GetInstance().AddCombo(50f);

                    // Taking Damage
                    objStat.TakeDamage(weaponDamage);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isAttacked)
            isAttacked = false;
    }
}