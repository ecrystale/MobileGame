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
    public Transform coinPrefab;
    public event Action<EnemySpawner, int, bool> WaveCleared;
    public event Action<EnemySpawner, int, bool> WaveEntered;
    public event Action<EnemySpawner> LevelInitialized;
    public event Action<EnemySpawner, EnemyHealth, EnemyMove> EnemySpawned;

    public int TotalEnemies => _totalEnemiesCount;

    [Tooltip("It is not used in the actual gameplay")]
    public TextAsset PreviewLevel;
    public Spawner[] SpawnersPreview;

    private LevelMeta _levelInfo;
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
        foreach (CollectibleBehavior coin in FindObjectsOfType<CollectibleBehavior>())
        {
            Destroy(coin.gameObject);
        }
        foreach (GameObject enemyGameObject in _activeEnemies)
        {
            Destroy(enemyGameObject);
        }
        _activeEnemies = new List<GameObject>();
        _spawners = level.Spawners;
        _levelInfo = level.Info;
        _wave = 0;
        _totalWave = _spawners.Length;

        _interval = PublicVars.WAVE_INIT_INTERVAL;
        _originalSpawnInterval = level.Info.Interval;

        _waveEnemiesCount = _spawners.Select(wave => wave.Enemies.Length).ToArray();
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
            Vector2 spawnOffset = spawner.Direction.normalized;
            Vector2 instantiatePosition = transform.position;
            Vector2 spawnPosition = spawnpts[spawner.SpawnPointIndex].transform.position;
            bool isLastWave = wave == _totalWave - 1;
            if (spawner.Centralized) spawnPosition -= spawnOffset * (1 + spawner.Enemies.Length / 2);

            for (int i = 0; i < spawner.Enemies.Length; i++)
            {
                currentEnemy = Instantiate(enemy[spawner.Enemies[i]], instantiatePosition, Quaternion.identity);

                EnemyMove enemyMove = currentEnemy.GetComponent<EnemyMove>();
                EnemyHealth enemyHealth = currentEnemy.GetComponent<EnemyHealth>();

                enemyMove.Spawner(spawnPosition, instantiatePosition, spawner.Duration);
                enemyHealth.SetHealth(Mathf.FloorToInt(enemyHealth.MaxHealth * _levelInfo.HealthFactor));

                enemyHealth.Destroyed += (EnemyHealth enemyHealth) => HandleDestroyedOrGone(wave, enemyHealth.gameObject, isLastWave, spawner.CoinReward, true);
                enemyMove.Gone += (EnemyMove enemyMove) => HandleDestroyedOrGone(wave, enemyMove.gameObject, isLastWave, spawner.CoinReward, false);

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

    void HandleDestroyedOrGone(int wave, GameObject enemy, bool isLastWave, int coinReward, bool destroyed)
    {
        if (destroyed) Instantiate(coinPrefab, enemy.transform.position, Quaternion.identity);
        _waveEnemiesCount[wave]--;
        _activeEnemies.Remove(enemy.gameObject);
        if (_waveEnemiesCount[wave] == 0)
        {
            if (WaveCleared != null) WaveCleared(this, wave, isLastWave);
        }
    }
}
