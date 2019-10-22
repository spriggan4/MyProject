using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemyPool = new List<GameObject>();

    public GameObject enemy = null;
    public GameObject target = null;
    public Transform[] points = null;

    public float spawnTime = 5f;
    public static bool isGameOver = false;

    public int enemyCount = 0;
    public int maxEnemy = 10;

    void Start()
    {
        for (int i = 0; i < maxEnemy; ++i)
        {
            enemy = (GameObject)Instantiate(enemy);
            enemy.name = "Enemy_" + i.ToString();
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }

        if (points.Length > 0)
        {
            StartCoroutine(CreateEnemy());
        }
    }

    IEnumerator CreateEnemy()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(spawnTime);

            if (isGameOver) yield break;

            foreach (GameObject enemy in enemyPool)
            {
                if (!enemy)
                {
                    if (!enemy.activeSelf)
                    {
                        int spawnPointIndex = Random.Range(0, points.Length);
                        enemy.transform.position = points[spawnPointIndex].position;
                        enemy.SetActive(true);

                        EnemyAIScript01 AI = enemy.GetComponent<EnemyAIScript01>();
                        if (target != null)
                        {
                            AI.SetTarget(target);
                        }

                        enemyCount++;
                        //Debug.Log("enemyCount : " + enemyCount);
                        break;
                    }
                }
            }
        }
    }
}
