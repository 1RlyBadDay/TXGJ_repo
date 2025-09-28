using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public int MAX_ENEMIES = 1;
    public bool spawning = true;
    int numberOfEnemies = 0;

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

    //Spawns enemies and updates their stats based on how many have been spawned
    void SpawnEnemy()
    {
        numberOfEnemies++;
        enemyPool.Add(Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity).gameObject);
        enemyPool[enemyPool.Count - 1].name = "Enemy " + numberOfEnemies;
        enemyPool[enemyPool.Count - 1].GetComponent<Enemy>().buffDamage(1f + (0.1f * (numberOfEnemies - 1)), 9999f); //increase damage by 10% for each enemy spawned after the first
        enemyPool[enemyPool.Count - 1].GetComponent<Enemy>().damageReduction = 1f - (0.05f * (numberOfEnemies - 1)); //reduce damage taken by 5% for each enemy spawned after the first
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
