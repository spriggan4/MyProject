using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMng : MonoBehaviour
{
    private Transform tr = null;
    private PickUp pickup = null;
    private QuickSlot slot = null;
    private QuickSlotImage slotImage = null;
    private Equipment equipment = null;

    private void Start()
    {
        tr = this.transform;
        pickup = this.GetComponent<PickUp>();
        if (pickup == null) { pickup = this.gameObject.AddComponent<PickUp>(); }
        slot = this.transform.GetComponentInChildren<QuickSlot>();
        equipment = tr.GetComponent<Equipment>();
        if (equipment == null) { equipment = tr.gameObject.AddComponent<Equipment>(); }
        slotImage = this.transform.GetComponentInChildren<QuickSlotImage>();
    }

    private void Update()
    {
        int slotNumber = 0;
        Item it = null;
        bool isPressedNumber = false;
        
        if (Input.GetKeyDown(KeyCode.F) && slot.IsCanPickUpItem()) { pickup.CheckItemInArea(tr.position); }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { slotNumber = 0; isPressedNumber = true; }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { slotNumber = 1; isPressedNumber = true; }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) { slotNumber = 2; isPressedNumber = true; }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) { slotNumber = 3; isPressedNumber = true; }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) { slotNumber = 4; isPressedNumber = true; }

        if (isPressedNumber)
        {
            isPressedNumber = false;
            // 그리고 누른 번호에 해당하는 슬롯의 정보를 현재 장착슬롯에 적용및 누른번호에 해당하는 슬롯에 대한 정보 소거
            // 따로 빼낸 정보를 슬롯에 추가
            AudioMng.GetInstance().PlaySound("Button1", tr.position, 100f);

            // 현재 낀 아이템이 있는지 체크
            if (equipment.IsEquipWeapon)
            {
                Item itMain = equipment.UnEquip();
                it = slot.GetItemListNumber(slotNumber);
                it.gameObject.SetActive(true);
                slot.RemoveItemMain();
                slot.AddItemMain(slot.ItemList[slotNumber]);
                slot.RemoveItemInNumber(slotNumber);
                slot.AddItem(slotNumber, itMain);
                equipment.Equip(it);                
            }
            else
            {
                slot.RemoveItemMain();
                it = slot.GetItemListNumber(slotNumber);
                slot.AddItemMain(it);
                slot.RemoveItemInNumber(slotNumber);
                slot.AddItemEmpty(slotNumber);
                equipment.Equip(it);
            }
        }
    }
}
