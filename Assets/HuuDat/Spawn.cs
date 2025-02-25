using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
  public GameObject[] enemyPrefabs; // Danh sách các quái có thể spawn
    public Transform[] spawnPoints; // Danh sách các điểm spawn
    public float spawnInterval = 5f; // Thời gian giữa các đợt spawn
    public int minEnemiesPerSpawn = 5; // Số quái ít nhất mỗi lần spawn
    public int maxEnemiesPerSpawn = 15; // Số quái tối đa mỗi lần spawn
    public float spawnRadius = 3f; // Bán kính để dàn trải quái tại mỗi điểm spawn

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnHorde();
            yield return new WaitForSeconds(spawnInterval); // Chờ rồi spawn đợt tiếp theo
        }
    }

    void SpawnHorde()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            int enemyCount = Random.Range(minEnemiesPerSpawn, maxEnemiesPerSpawn + 1);

            for (int i = 0; i < enemyCount; i++)
            {
                // Chọn ngẫu nhiên loại quái
                GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                // Chọn vị trí random trong bán kính spawnRadius
                Vector3 randomOffset = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
                Vector3 spawnPosition = spawnPoint.position + randomOffset;

                // Spawn quái
                Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            }
        }
    }
}
