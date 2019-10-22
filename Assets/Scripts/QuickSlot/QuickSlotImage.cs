using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotImage : MonoBehaviour
{
    [SerializeField] private GameObject slot0 = null;
    [SerializeField] private GameObject slot1 = null;
    [SerializeField] private GameObject slot2 = null;
    [SerializeField] private GameObject slot3 = null;
    [SerializeField] private GameObject slot4 = null;
    [SerializeField] private GameObject slotMain = null;
    [SerializeField] private Sprite defaultSprite = null;
    private Image imageSlot0 = null;
    private Image imageSlot1 = null;
    private Image imageSlot2 = null;
    private Image imageSlot3 = null;
    private Image imageSlot4 = null;
    private Image imageSlotMain = null;
    

    private void Awake()
    {
        if (slot0 != null) imageSlot0 = slot0.GetComponent<Image>();
        if (slot1 != null) imageSlot1 = slot1.GetComponent<Image>();
        if (slot2 != null) imageSlot2 = slot2.GetComponent<Image>();
        if (slot3 != null) imageSlot3 = slot3.GetComponent<Image>();
        if (slot4 != null) imageSlot4 = slot4.GetComponent<Image>();
        if (slotMain != null) imageSlotMain = slotMain.GetComponent<Image>();
    }
    public void Regist(int slotNum, Sprite spriteImage)
    {
        if (spriteImage == null) { return; }
        switch (slotNum)
        {
            case 0:
                imageSlot0.sprite = spriteImage;
                break;
            case 1:
                imageSlot1.sprite = spriteImage;
                break;
            case 2:
                imageSlot2.sprite = spriteImage;
                break;
            case 3:
                imageSlot3.sprite = spriteImage;
                break;
            case 4:
                imageSlot4.sprite = spriteImage;
                break;
        }
    }
    public void RegistMain(Sprite spriteImage)
    {
        imageSlotMain.sprite = spriteImage;
    }
    public void SetColor(int slotNum , Color color)
    {
        switch (slotNum)
        {
            case 0:
                imageSlot0.color = color;
                break;
            case 1:
                imageSlot1.color = color;
                break;
            case 2:
                imageSlot2.color = color;
                break;
            case 3:
                imageSlot3.color = color;
                break;
            case 4:
                imageSlot4.color = color;
                break;
        }
    }
    public void SetColorMain(Color color)
    {
        imageSlotMain.color = color;
    }
    public Color GetColor(int slotNum)
    {
        switch (slotNum)
        {
            case 0:
                return imageSlot0.color;
            case 1:
                return imageSlot1.color;
            case 2:
                return imageSlot2.color;
            case 3:
                return imageSlot3.color;
            case 4:
                return imageSlot4.color;
        }
        return Color.black;
    }
    public Color GetColorMain()
    {
        return imageSlotMain.color;
    }
    public void RemoveAt(int slotNum)
    {
        switch (slotNum)
        {
            case 0: imageSlot0.sprite = defaultSprite; break;
            case 1: imageSlot1.sprite = defaultSprite; break;
            case 2: imageSlot2.sprite = defaultSprite; break;
            case 3: imageSlot3.sprite = defaultSprite; break;
            case 4: imageSlot4.sprite = defaultSprite; break;
        }
    }
    public void RemoveMain()
    {
        imageSlotMain.sprite = defaultSprite;
    }
}