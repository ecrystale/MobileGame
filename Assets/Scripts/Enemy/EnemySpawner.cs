using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemy;
    public GameObject[] spawnpts;
    public float interval = 2f;
    public TextAsset textFile;
    public bool Active = false;
    public event Action<EnemySpawner, int, bool> WaveCleared;
    public event Action<EnemySpawner, int, bool> WaveEntered;

    [ReadOnly]
    public Spawner[] _spawners;
    private float _originalSpawnInterval;
    private int _wave;
    private int _totalWave;
    private int[] _waveEnemiesCount;
    private List<GameObject> _activeEnemies = new List<GameObject>();

    public void Reset(Spawner[] spawners)
    {
        Active = false;

        FindObjectOfType<ObjectPooler>()?.CleanAll();
        foreach (GameObject enemyGameObject in _activeEnemies)
        {
            Destroy(enemyGameObject);
        }
        _activeEnemies = new List<GameObject>();
        _spawners = spawners;
        _wave = 0;
        _totalWave = _spawners.Length;
        _originalSpawnInterval = interval;
        _waveEnemiesCount = _spawners.Select(wave => wave.Enemies.Length - 1).ToArray();
    }

    void Start()
    {
        WaveCleared += (spawner, wave, isLastWave) => Debug.Log($"{wave}{(isLastWave ? " (last wave)" : "")} is cleared!");
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active) return;

        interval -= Time.deltaTime;
        if ((interval <= 0) && (_wave < _totalWave))
        {
            GameObject currentEnemy;
            int wave = _wave;
            Spawner spawner = _spawners[wave];
            Vector3 spawnOffset = spawner.Direction.normalized;
            Vector3 instantiatePosition = transform.position;
            Vector3 spawnPosition = spawnpts[spawner.SpawnPointIndex].transform.position;
            bool isLastWave = wave == _totalWave - 1;
            if (spawner.Centralized) spawnPosition -= spawnOffset * (1 + spawner.Enemies.Length / 2);

            for (int i = 0; i < spawner.Enemies.Length; i++)
            {
                currentEnemy = Instantiate(enemy[spawner.Enemies[i]], instantiatePosition, Quaternion.identity);
                _activeEnemies.Add(currentEnemy);
                currentEnemy.GetComponent<EnemyMove>().Spawner(spawnPosition, instantiatePosition, spawner.Duration);
                spawnPosition += spawnOffset;
                currentEnemy.GetComponent<EnemyHealth>().Destroyed += (EnemyHealth enemyHealth) => HandleDestroyed(wave, enemyHealth, isLastWave);
            }

            if (WaveEntered != null) WaveEntered(this, wave, isLastWave);
            if (isLastWave) Active = false;

            _wave += 1;

            interval = _originalSpawnInterval;
        }
    }

    void HandleDestroyed(int wave, EnemyHealth enemyHealth, bool isLastWave)
    {
        _waveEnemiesCount[wave]--;
        _activeEnemies.Remove(enemyHealth.gameObject);
        if (_waveEnemiesCount[wave] == 0)
        {
            if (WaveCleared != null) WaveCleared(this, wave, isLastWave);
        }
    }
}
