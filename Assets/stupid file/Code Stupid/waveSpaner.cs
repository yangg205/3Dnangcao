using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
[System.Serializable]
public class wave
{
    public string waveName;
    public int noOfEnemies;
    public GameObject[] typeOfEnemies;
    public float spawnerInterval;

}
public class waveSpaner : MonoBehaviour
{

    public wave[] Waves;
    public Transform[] sqawnPoints;
    private wave currentWave;
    private int currentWaveNumber;
    private float nextSpawntime;
    private bool canSpawn = true;
    public Animator animator;
    public Text wave_name;
    private bool canAnimtate = false;
    private void Update()
    {
        currentWave = Waves[currentWaveNumber];
        SpawmWawe();
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (totalEnemies.Length == 0)
        {
            if (currentWaveNumber + 1 != Waves.Length)
            {
                if (canAnimtate)
                {
                    wave_name.text = Waves[currentWaveNumber + 1].waveName;
                    animator.SetTrigger("WaveComplete");
                    canAnimtate = false;
                }

            }
            else 
            {
                Debug.Log("GameFinshing");
            }
        }
    }
    void spawnWaweNextWave()
    {
        currentWaveNumber++;
        canSpawn = true;
    }
    void SpawmWawe()
    {
        if (canSpawn && nextSpawntime < Time.time)
        {
            GameObject randomEnemy = currentWave.typeOfEnemies[Random.Range(0, currentWave.typeOfEnemies.Length)];
            Transform randomPoint = sqawnPoints[Random.Range(0, sqawnPoints.Length)];
            Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
            currentWave.noOfEnemies--;
            nextSpawntime = Time.time + currentWave.spawnerInterval;
            if (currentWave.noOfEnemies == 0)
            {
                canSpawn = false;
                canAnimtate = true;
            }
        }
    }
}