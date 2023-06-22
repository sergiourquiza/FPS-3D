using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public GameObject[] EnemieTypes;
    public int MaxEnemies = 10;
    public float SpawnTime = 60f;
    public float MinSpawnTime = 10f;
    public float SpawnRadius = 5f;
    private GameObject SpawnPoint;

    private void Start()
    {
        SpawnPoint = GameObject.Find("SpawnPoint");
        StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (SpawnTime >= MinSpawnTime)
        {
            yield return new WaitForSeconds(Random.Range(1f, SpawnTime));

            if(SpawnTime > 10){
                SpawnTime -= 5f;
            }
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        int maxSpawnCount = Random.Range(1, Mathf.Min(MaxEnemies, EnemieTypes.Length) + 1);

        for (int i = 0; i < maxSpawnCount; i++)
        {
            int randomIndex = Random.Range(0, EnemieTypes.Length);
            GameObject enemyType = EnemieTypes[randomIndex];

            var spawnPosition = SpawnPoint.transform.position + Random.insideUnitSphere * SpawnRadius;
            spawnPosition.y = 0f;

            Instantiate(enemyType, spawnPosition, Quaternion.identity);
        }
    }
}