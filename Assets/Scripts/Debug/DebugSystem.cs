using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugSystem : MonoBehaviour
{
    private const int FONTSIZE = 16;
    private const float SIZEDELTA_WIDTH = 150f;
    private const float SIZEDELTA_HEIGHT = 150f;
    private const float OFFSET_X_INIT = -210f;
    private const float OFFSET_Y_INIT = -175f;
    private const float OFFSET_X_ADD = 170f;
    private static DebugSystem instance = null;
    private RectTransform rctTr = null;
    private GameObject textUiMainSlot = null;
    private GameObject[] textUiQuickSlot = null;
    private void Start()
    {
        rctTr = this.GetComponent<RectTransform>();
        if (rctTr == null) { Debug.LogError("rctTr is null"); Debug.Break(); }
        textUiQuickSlot = new GameObject[QuickSlot.SLOTMAX];
    }
    public static DebugSystem GetInstance()
    {
        if (!instance)
        {
            instance = (DebugSystem)GameObject.FindObjectOfType(typeof(DebugSystem));
            if (!instance)
                Debug.LogError("There needs to be one active MyClass script on a GameObject in your scene.");
        }
        return instance;
    }
    public void ShowQuickSlotMain(string strTxt)
    {
        if (!textUiMainSlot)
            textUiMainSlot = CreateTextUI(new Vector3(-405f, -103f, 0f), strTxt);
        else
            SetTextUiText(textUiMainSlot, strTxt);
    }
    public void ShowQuickSlot(int num , string strTxt)
    {
        if (!textUiQuickSlot[num])
            textUiQuickSlot[num] = CreateTextUI(new Vector3(OFFSET_X_INIT + OFFSET_X_ADD * num, OFFSET_Y_INIT, 0f), strTxt);
        else
            SetTextUiText(textUiQuickSlot[num], strTxt);
    }
    public void HideQuickSlotMain()
    {
        if (!textUiMainSlot)
            Debug.LogError("textUiMainSlot is Null,,");
        Destroy(textUiMainSlot);
    }
    public void HideQuickSlot(int num)
    {
        if (!textUiQuickSlot[num])
            Debug.LogError("textUiQuickSlot[" + num + "] is Null,,,");
        Destroy(textUiQuickSlot[num]);
    }
    private GameObject CreateTextUI(Vector3 uiPos , string strTxt)
    {
        GameObject textUi = new GameObject("DebugText");
        RectTransform rctTextUi = textUi.AddComponent<RectTransform>();
        Text txtTextUi = textUi.AddComponent<Text>();
        textUi.transform.SetParent(instance.transform);
        
        txtTextUi.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        txtTextUi.fontSize = FONTSIZE;
        txtTextUi.verticalOverflow = VerticalWrapMode.Overflow;
        txtTextUi.color = Color.red;
        txtTextUi.text = strTxt;

        uiPos.x += 512f;
        uiPos.y += 384f;
        rctTextUi.position = uiPos;
        rctTextUi.sizeDelta = new Vector2(SIZEDELTA_WIDTH , SIZEDELTA_HEIGHT);

        return textUi;
    }
    private void SetTextUiText(GameObject textUi , string strTxt)
    {
        if (!textUi)
            Debug.LogError("textUi is Null..");
        Text txtTextUi = textUi.GetComponent<Text>();
        if (!txtTextUi)
            Debug.LogError("txtTextUi is Null.. Maybe This GameObject is Not GUI's GameObject..");
        txtTextUi.text = strTxt;
    }
}