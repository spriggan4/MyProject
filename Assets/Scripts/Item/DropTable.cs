using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTable : MonoBehaviour {
    [System.Serializable]
    public class DropCurrency {
        public string name;
        public GameObject item;
        public float dropDice;
    }
    public List<DropCurrency> dropTable = new List<DropCurrency>();
    private Transform tr = null;

    private void Start()
    {
        tr = this.transform;
    }

    public void GetRandomItem()
    {
        float rand = Random.Range(0f, 1f);
        float tempMin = 0f;
        float tempMax = 0f;
        //Debug.Log(rand);
        for (int i = 0; i < dropTable.Count; ++i)
        {
            tempMin = tempMax;
            tempMax += dropTable[i].dropDice;
            if (rand > tempMin && rand < tempMax)
            {
                Item.Create(dropTable[i].item, tr.position, tr.rotation);
                return;
            }
        }
    }
}