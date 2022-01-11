using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadar : MonoBehaviour
{
    private GameObject[] enemies;
    private Transform[] enemiesInRange;
    public Transform closestEnemy;
    public bool enemyContact;

    // Start is called before the first frame update
    void Start()
    {
        closestEnemy = null;
        enemyContact = false;
    }

    // Update is called once per frame
    void Update()
    {
        closestEnemy = getClosestEnemy();
    }

    public Transform[] GetEnemiesInRange(float range)
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        bool[] isInRange = new bool[enemies.Length];

        for(int i = 0; i < isInRange.Length; i++)
        {
            isInRange[i] = false;
        }

        int countInRange = 0;

        for (int i = 0; i < isInRange.Length; i++)
        {
            float currentDistance = Vector3.Distance(transform.position, enemies[i].transform.position);
            if(currentDistance <= range)
            {
                isInRange[i] = true;
                countInRange++;
            }
        }

        if(countInRange == 0)
        {
            return null;
        }
 
        enemiesInRange = new Transform[countInRange];
        int addedCount = 0;

        for(int i = 0; i < isInRange.Length; i++)
        {
            if(isInRange[i])
            {
                enemiesInRange[addedCount] = enemies[i].transform;
                addedCount++;
            }
        }

        return enemiesInRange;
    }

    public Transform getClosestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform trans = null;

        foreach (GameObject go in enemies) {
            float currentDistance = Vector3.Distance(transform.position, go.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                trans = go.transform;
            }
        }

        return trans;
    }
}
