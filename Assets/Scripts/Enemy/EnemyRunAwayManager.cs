using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunAwayManager : MonoBehaviour
{

    public EnemyStats[] enemies;
    //public int enemiesLength;
    // Use this for initialization
    void Start()
    {
        enemies = this.gameObject.GetComponentsInChildren<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Length == 0)
        {
            return;
        }

        if (enemies.Length <= 1)
        {            
            enemies[1].gameObject.GetComponent<EnemyAIScript01>().runAway = true;
        }
    }
}
