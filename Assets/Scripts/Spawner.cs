using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Spawner : MonoBehaviour
{

    public GameObject[] enemy;
    public float respawnTime = 3f;
    public int enemySpawnCount = 10;
    public GameController gameController;

    private bool lastEnemySpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawner());   
    }

    // Update is called once per frame
    void Update()
    {
        if(lastEnemySpawned && FindObjectOfType<EnemyScript>() == null)
        {
            StartCoroutine(gameController.LevelComplete());
        }
    }

    IEnumerator EnemySpawner()
    {
        for(int i = 0; i < enemySpawnCount; i++)
        {
            yield return new WaitForSeconds(respawnTime);
            SpawnEnemy();
        }
        
        lastEnemySpawned = true;
    }

    void SpawnEnemy()
    {
        int index = Random.Range(0, enemy.Length);
        Vector2 RandomPos = new Vector2(Random.Range(-1.8f, 1.8f), transform.position.y);
        Instantiate(enemy[index], RandomPos, Quaternion.identity);
    }

}
