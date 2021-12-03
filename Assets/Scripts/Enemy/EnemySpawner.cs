using UnityEngine;
using System;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemy;
    public GameObject[] spawnpts;
    public float interval = 2f;
    public string filename;
    public TextAsset textFile;
    public event Action<EnemySpawner, int, bool> WaveCleared;

    private Spawner[] _spawners;
    private float _originalSpawnInterval;
    private int _wave;
    private int _totalWave;
    private int[] _waveEnemiesCount;

    void Start()
    {
        _spawners = ParseStages(filename);
        Debug.Log(JsonUtility.ToJson(new JsonWrapper<Spawner>(_spawners), true));

        _wave = 0;
        _totalWave = _spawners.Length;
        _originalSpawnInterval = interval;
        _waveEnemiesCount = _spawners.Select(wave => wave.Enemies.Length - 1).ToArray();
        WaveCleared += (spawner, wave, isLastWave) => Debug.Log($"{wave}{(isLastWave ? " (last wave)" : "")} is cleared!");
    }

    // Update is called once per frame
    void Update()
    {
        interval -= Time.deltaTime;
        if ((interval <= 0) && (_wave < _totalWave))
        {
            GameObject currentEnemy;
            int wave = _wave;
            Spawner spawner = _spawners[wave];
            Vector3 spawnOffset = spawner.Direction.normalized;
            Vector3 instantiatePosition = transform.position;
            Vector3 spawnPosition = spawnpts[spawner.SpawnPointIndex].transform.position;
            if (spawner.Centralized) spawnPosition -= spawnOffset * (1 + spawner.Enemies.Length / 2);

            for (int i = 0; i < spawner.Enemies.Length; i++)
            {
                currentEnemy = Instantiate(enemy[spawner.Enemies[i]], instantiatePosition, Quaternion.identity);
                currentEnemy.GetComponent<EnemyMove>().Spawner(spawnPosition, instantiatePosition, spawner.Duration);
                spawnPosition += spawnOffset;
                currentEnemy.GetComponent<EnemyHealth>().Destroyed += (EnemyHealth enemyHealth) => HandleDestroyed(wave, enemyHealth, wave == _totalWave - 1);
            }

            _wave += 1;

            interval = _originalSpawnInterval;
        }
    }

    Spawner[] ParseStages(string stageFile)
    {
        string[] rawStages;
        if (textFile)
        {
            rawStages = textFile.text.Split('\n');
            /*foreach(string line in rawStages){
              print(line);
            }*/

            return rawStages.Select(rawStage =>
            {
                // Format:
                //    0000000          v           1           false         1
                // enemy indexes   direction  spawn index   centralized   duration
                //    0000000        1|2         1             true          -1
                Spawner spawner = new Spawner();

                string[] segments = rawStage.Split();
                if (segments.Length < 5) throw new ParsingException($"5 arguments excepted, but {segments.Length} arguments are given in {stageFile}");
                try
                {
                    spawner.Enemies = segments[0].Select(enemyChar => Int32.Parse(enemyChar.ToString())).ToArray();
                    spawner.SpawnPointIndex = Int32.Parse(segments[2]);
                    spawner.Centralized = bool.Parse(segments[3]);
                    spawner.Duration = Int32.Parse(segments[4]);
                }
                catch
                {
                    throw new ParsingException($"Failed to parse line \"{rawStage}\" in {stageFile}");
                }

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

        return new Spawner[0];
    }

    void HandleDestroyed(int wave, EnemyHealth enemyHealth, bool isLastWave)
    {
        Debug.Log($"Destroyed 1 at wave {wave}, {_waveEnemiesCount[wave]} remaining");
        _waveEnemiesCount[wave]--;
        if (_waveEnemiesCount[wave] == 0)
        {
            if (WaveCleared != null) WaveCleared(this, wave, isLastWave);
        }
    }
}
