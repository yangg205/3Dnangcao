using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class waveSpaner : MonoBehaviour
{
    [System.Serializable]
    public class waveContent
    {
        [SerializeField][NonReorderable] GameObject[] zombieSpawn;
        public GameObject[] getZombieSpawnList()
        {
            return zombieSpawn;
        }
    }
    [SerializeField][NonReorderable] waveContent[] waves;
    int currentWave = 0;
    float spawnRange = 10;
    public List<GameObject> CurrentZombie;
    public int Enemieskilled;
    void Start()
    {
        SpawnWave();
    }
    void Update()
    {
        if (CurrentZombie.Count ==0)
        {
            currentWave++;
            SpawnWave();
        }
    }
    void SpawnWave()
    {

        for (int i = 0; i < waves[currentWave].getZombieSpawnList().Length; i++)
        {
            GameObject newspawn = Instantiate(waves[currentWave].getZombieSpawnList()[i], FindSpawnLoc(), Quaternion.identity);
            CurrentZombie.Add(newspawn);

            ZombieAi monster = newspawn.GetComponent<ZombieAi>();
            monster.setSpawner(this);
        }

    }
    Vector3 FindSpawnLoc()
    {
        Vector3 Spawnpos;

        float xLoc = Random.Range(-spawnRange, spawnRange) + transform.position.x;
        float zloc = Random.Range(-spawnRange, spawnRange) + transform.position.z;
        float yloc = transform.position.y;

        Spawnpos = new Vector3(xLoc, yloc, zloc);
        if (Physics.Raycast(Spawnpos, Vector3.down, 5))
        {
            return Spawnpos;
        }
        else
        {
            return FindSpawnLoc();
        }

    }
}
