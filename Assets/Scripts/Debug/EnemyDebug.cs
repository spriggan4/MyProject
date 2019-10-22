using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDebug : MonoBehaviour
{
    public GameObject textPrefab = null;
    public Transform textTr = null;
    private Transform target = null;
    private Text enemyText = null;
    private CharacterStat stat = null;
    private EnemyAIScript01 enemy = null;
    private Transform player = null;

    private int enemyCnt = 0;

    void Start()
    {
        target = this.GetComponent<Transform>();
        stat = this.gameObject.GetComponent<CharacterStat>();
        enemy = this.gameObject.GetComponent<EnemyAIScript01>();
        GameObject goPlayer = GameObject.FindGameObjectWithTag("Player");
        if (goPlayer == null)
        {
            Debug.LogError("Cant Find Player...");
            Debug.Break();
        }
        player = goPlayer.transform;
        

        if (target == null) { Debug.LogError("Target is null"); }
        if (player == null) { Debug.LogError("player is null"); }

        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                target = Instantiate(textPrefab, c.transform).transform;
                enemyText = target.GetChild(0).GetComponent<Text>();
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //DisplayHP();
    }

    private void DisplayHP()
    {
        if (!enemyText || !player)
        {
            return;
        }

        enemyText.text = ("\nTarget :" + player.name +
                          "\nHP : " + stat.currentHealth + " / " + stat.maxHealth +
                          "\nStatus\n" + "Range : " + enemy.attackRange +
                          "\nAttackTime : " + enemy.attackTime);
        enemyText.fontSize = 14;
        enemyText.color = Color.red;
        enemyText.fontStyle = FontStyle.Bold;
        Vector3 pos = Camera.main.WorldToScreenPoint(textTr.position);

        enemyText.transform.position = new Vector3(pos.x, pos.y + 5, pos.z);

        if (stat.currentHealth <= 0)
        {
            Destroy(enemyText);
        }
    }
}
