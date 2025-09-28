using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public int MAX_ENEMIES = 1;
    public bool spawning = true;

    [Header("debugging ")]
    public bool debugging = false;
    public bool killAllEnemies = false;

    public List<GameObject> enemyPool = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (killAllEnemies)
        {
            // Create a new list to store enemies we need to destroy
            var enemiesToKill = new List<GameObject>(enemyPool);
            
            // Iterate over the copy of the list
            foreach (GameObject enemy in enemiesToKill)
            {
                if (enemy != null)
                {
                    enemy.GetComponent<Enemy>().Dying();
                }
            }
            
            enemyPool.Clear();
            killAllEnemies = false;
        }
        
        if (enemyPool.Count < MAX_ENEMIES && spawning)
        {
            SpawnEnemy();
        }
    }
    void SpawnEnemy()
    {
        enemyPool.Add(Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity).gameObject);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        if(enemyPool.Contains(enemy)){
            enemyPool.Remove(enemy);
        }
        else
        {
            Debug.LogError("Enemy not found in pool");
        }
    }
}
