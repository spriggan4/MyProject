using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(CharacterStat))]
public class HealthUI : MonoBehaviour
{
    public GameObject uiPrefab = null;
    public Transform target = null;

    private float visibleTime = 10f;
    private float lastMadeVisibleTime = 0f;

    private Transform ui = null;
    private Image healthSlider = null;

    private CharacterStat stat = null;

    private const float OFFSET_Y = 140.0f;

    private void Awake()
    {
        stat = this.transform.GetComponent<CharacterStat>();
    }

    private void Start()
    {
        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.WorldSpace)
            {
                ui = Instantiate(uiPrefab, c.transform).transform;

                healthSlider = ui.GetChild(0).GetComponent<Image>();
                ui.gameObject.SetActive(true);
                break;
            }
        }
        
        GetComponent<CharacterStat>().OnHealthChanged += OnHealthChanged;
    }

    private void LateUpdate()
    {
        if (ui != null)
        {
            ui.position = target.position;
            // 일정 시간 후 체력바 사라짐
            if (Time.time - lastMadeVisibleTime > visibleTime)
            {
                //ui.gameObject.SetActive(false); 
            }
        }
    }

    private void OnHealthChanged(float maxHealth, float currentHealth)
    {
        if (ui != null)
        {
            ui.gameObject.SetActive(true);
            lastMadeVisibleTime = Time.time;
            float healthPercent = (float)currentHealth / maxHealth;
            healthSlider.fillAmount = healthPercent;
            if (currentHealth <= 0)
            {
                Destroy(ui.gameObject);
            }
        }
    }
}
