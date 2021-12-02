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

    private Spawner[] _spawners;
    private float _originalSpawnInterval;
    private int _wave;
    private int _totalWave;

    void Start()
    {
        _spawners = ParseStages(filename);
        Debug.Log(JsonUtility.ToJson(new JsonWrapper<Spawner>(_spawners), true));

        _wave = 0;
        _totalWave = _spawners.Length;
        _originalSpawnInterval = interval;
    }

    // Update is called once per frame
    void Update()
    {
        interval -= Time.deltaTime;
        if ((interval <= 0) && (_wave < _totalWave))
        {
            GameObject currentEnemy;
            Vector3 spawnOffset = _spawners[_wave].Direction.normalized;
            Vector3 instantiatePosition = transform.position;
            Vector3 spawnPosition = spawnpts[_spawners[_wave].SpawnPointIndex].transform.position;
            if (_spawners[_wave].Centralized) spawnPosition -= spawnOffset * (1 + _spawners[_wave].Enemies.Length / 2);

            for (int i = 0; i < _spawners[_wave].Enemies.Length; i++)
            {
                currentEnemy = Instantiate(enemy[_spawners[_wave].Enemies[i]], instantiatePosition, Quaternion.identity);
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
