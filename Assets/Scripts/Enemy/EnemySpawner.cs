using UnityEngine;
using System;
using System.Linq;
using System.IO;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemy;
    public GameObject[] spawnpts;
    public float interval = 2f;
    public string filename;
    public Spawner[] Spawners;

    private string[] spawn;
    private float _originalSpawnInterval;
    private int _wave;
    private int _totalWave;

    void Start()
    {
        Spawners = ParseStages(filename);
        Debug.Log(JsonUtility.ToJson(new JsonWrapper<Spawner>(Spawners), true));

        _wave = 0;
        _totalWave = Spawners.Length;
        _originalSpawnInterval = interval;
    }

    // Update is called once per frame
    void Update()
    {
        interval -= Time.deltaTime;
        if ((interval <= 0) && (_wave < _totalWave))
        {
            GameObject currentEnemy;
            Vector3 spawnOffset = Spawners[_wave].Direction.normalized;
            Vector3 instantiatePosition = transform.position;
            Vector3 spawnPosition = spawnpts[Spawners[_wave].SpawnPointIndex].transform.position;
            if (Spawners[_wave].Centralized) spawnPosition -= spawnOffset * (1 + Spawners[_wave].Enemies.Length / 2);

            for (int i = 0; i < Spawners[_wave].Enemies.Length; i++)
            {
                currentEnemy = Instantiate(enemy[Spawners[_wave].Enemies[i]], instantiatePosition, Quaternion.identity);
                currentEnemy.GetComponent<EnemyMove>().Spawner(spawnPosition);
                spawnPosition += spawnOffset;
            }

            _wave += 1;
            interval = _originalSpawnInterval;
        }
    }

    Spawner[] ParseStages(string stageFile)
    {
        string[] rawStages = File.ReadAllLines(PublicVars.GetStagePath(stageFile));

        return rawStages.Select(rawStage =>
        {
            // Format:
            //    0000000          v           1           false
            // enemy indexes   direction  spawn index   centralized
            //    0000000        1|2         1             true
            Spawner spawner = new Spawner();

            string[] segments = rawStage.Split();

            spawner.Enemies = segments[0].Select(enemyChar => Int32.Parse(enemyChar.ToString())).ToArray();
            spawner.SpawnPointIndex = Int32.Parse(segments[2]);
            spawner.Centralized = bool.Parse(segments[3]);

            if (segments[1].Length == 1)
            {
                spawner.Direction = segments[1] == "v" ? Vector2.down : Vector2.right;
            }
            else
            {
                float[] components = segments[1].Split('|').Select(component => float.Parse(component)).ToArray();
                spawner.Direction = new Vector2(components[0], components[1]);
            }
            return spawner;
        }).ToArray();
    }
}
