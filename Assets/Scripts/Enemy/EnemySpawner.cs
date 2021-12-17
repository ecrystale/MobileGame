using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemy;
    public GameObject[] spawnpts;
    public bool Active = false;
    public event Action<EnemySpawner, int, bool> WaveCleared;
    public event Action<EnemySpawner, int, bool> WaveEntered;
    public event Action<EnemySpawner> LevelInitialized;
    public event Action<EnemySpawner, EnemyHealth, EnemyMove> EnemySpawned;

    public int TotalEnemies => _totalEnemiesCount;

    [Tooltip("It is not used in the actual gameplay")]
    public TextAsset PreviewLevel;
    public Spawner[] SpawnersPreview;

    private Spawner[] _spawners;
    private float _interval = 2f;
    private float _originalSpawnInterval;
    private int _wave;
    private int _totalWave;
    private int[] _waveEnemiesCount;
    private int _totalEnemiesCount;
    private List<GameObject> _activeEnemies = new List<GameObject>();

    public void Reset(Level level)
    {
        Active = false;

        FindObjectOfType<ObjectPooler>()?.CleanAll();
        foreach (GameObject enemyGameObject in _activeEnemies)
        {
            Destroy(enemyGameObject);
        }
        _activeEnemies = new List<GameObject>();
        _spawners = level.Spawners;
        _wave = 0;
        _totalWave = _spawners.Length;

        _interval = PublicVars.WAVE_INIT_INTERVAL;
        _originalSpawnInterval = level.Info.Interval;

        _waveEnemiesCount = _spawners.Select(wave => wave.Enemies.Length - 1).ToArray();
        _totalEnemiesCount = _waveEnemiesCount.Sum();

        if (LevelInitialized != null) LevelInitialized(this);
    }

    void Start()
    {
        WaveCleared += (spawner, wave, isLastWave) => Debug.Log($"{wave}{(isLastWave ? " (last wave)" : "")} is cleared!");
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active) return;

        _interval -= Time.deltaTime;
        if ((_interval <= 0) && (_wave < _totalWave))
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

                EnemyMove enemyMove = currentEnemy.GetComponent<EnemyMove>();
                EnemyHealth enemyHealth = currentEnemy.GetComponent<EnemyHealth>();

                enemyMove.Spawner(spawnPosition, instantiatePosition, spawner.Duration);

                enemyHealth.Destroyed += (EnemyHealth enemyHealth) => HandleDestroyed(wave, enemyHealth, isLastWave, spawner.CoinReward);

                if (EnemySpawned != null) EnemySpawned(this, enemyHealth, enemyMove);

                _activeEnemies.Add(currentEnemy);
                spawnPosition += spawnOffset;
            }

            if (WaveEntered != null) WaveEntered(this, wave, isLastWave);
            if (isLastWave) Active = false;

            _wave += 1;

            _interval = _originalSpawnInterval;
        }
    }

    void HandleDestroyed(int wave, EnemyHealth enemyHealth, bool isLastWave, int coinReward)
    {
        Debug.Log(coinReward);
        _waveEnemiesCount[wave]--;
        _activeEnemies.Remove(enemyHealth.gameObject);
        if (_waveEnemiesCount[wave] == 0)
        {
            if (WaveCleared != null) WaveCleared(this, wave, isLastWave);
        }
    }
}
