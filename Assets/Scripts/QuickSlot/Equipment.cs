using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private GameObject rightHand = null;
    private Weapon equippedItem = null;
    private PlayerAtkMng playerAtkMng = null;
    private bool isEquipWeapon = false;
    public bool IsEquipWeapon
    {
        get { return isEquipWeapon; }
        set { isEquipWeapon = value; }
    }
    public Item GetEquippedItem { get { return equippedItem; } }

    private void Start()
    {
        playerAtkMng = this.GetComponent<PlayerAtkMng>();
    }

    public void Equip(Item it)
    {
        if (!isEquipWeapon)
        {
            if (it == null) { Debug.LogError("it is null"); }

            GameObject go = rightHand;
            BoxCollider boxCollider = it.GetComponent<BoxCollider>();

            if (go == null) { Debug.LogError("go is null"); }
            if (!it.gameObject.activeInHierarchy) { it.gameObject.SetActive(true); }
            if (boxCollider != null) { Destroy(boxCollider); }

            it.transform.parent = go.transform;
            it.transform.position = go.transform.position;
            equippedItem = it.transform.GetComponent<Weapon>();
            isEquipWeapon = true;
            equippedItem.IsPlayerEquipped = true;

            if (equippedItem != null)
            {
                playerAtkMng.EquippedWeapon = equippedItem;   
                playerAtkMng.IsEquippedWeapon = isEquipWeapon;
                playerAtkMng.SetForAtk();
            }
        }
    }

    public Item UnEquip()
    {
        if (isEquipWeapon)
        {
            if (equippedItem != null)
            {
                Debug.Log("UnEquip 실행");
                isEquipWeapon = false;
                playerAtkMng.IsEquippedWeapon = isEquipWeapon;
                
                equippedItem.gameObject.SetActive(false);
                equippedItem.DestoryWeaponMesh();
                equippedItem.StopAtking();
                return equippedItem;
            }
        }
        return null;
    }
}
