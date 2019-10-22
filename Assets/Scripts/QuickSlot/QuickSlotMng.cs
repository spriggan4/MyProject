using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(QuickSlot))]
[RequireComponent(typeof(QuickSlotImage))]
public class QuickSlotMng : MonoBehaviour
{
    private Transform tr = null;
    private QuickSlot slot = null;
    private QuickSlotImage slotImage = null;
    private PickUp pickup = null;
    private Equipment equipment = null;

    private void Start()
    {
        tr = this.transform;
        slot = GetComponent<QuickSlot>();
        slotImage = GetComponent<QuickSlotImage>();
        pickup = tr.parent.GetComponent<PickUp>();
        if (pickup == null) { pickup = tr.parent.gameObject.AddComponent<PickUp>(); }
        equipment = tr.parent.GetComponent<Equipment>();
        if (equipment == null) { equipment = tr.parent.gameObject.AddComponent<Equipment>(); }
    }
    private void Update()
    {
        if (pickup.IsExistAroundItem)
        {
            pickup.IsExistAroundItem = false;
            if (!equipment.IsEquipWeapon)
            {
                slot.AddItemMain(pickup.GetPickupItem);
                equipment.Equip(pickup.GetPickupItem);
            }
            else
            {
                slot.AddItem(slot.GetEmptySlot(), pickup.GetPickupItem);
                pickup.GetPickupItem.gameObject.SetActive(false);
            }
        }
        
        if (slot.GetItemMain())
        {
            Weapon weapon = slot.GetItemMain().GetComponent<Weapon>();
            slotImage.SetColorMain(weapon.SlotColor);
        }

        if (slot.GetItemMain() && slot.GetItemMain().IsDestroyed)
        {
            Item itMainTemp = slot.GetItemMain();
            //Weapon weaponMainTemp = itMainTemp.GetComponent<Weapon>();
            //if (!weaponMainTemp)
            //    Debug.LogError("weaponMainTemp is NULL,,");
            equipment.UnEquip();
            slot.RemoveItemMain();
            itMainTemp.DestroyItem();
        }
    }
}
