using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TransitionManager))]
public class Game : MonoBehaviour
{
    public static Game CurrentGame;

    public PlayerData PlayerData { get; set; }
    //public string[] Levels = { "Main", "ModifyPatterns" };
    public TextAsset[] LevelFiles;
    public MenuManager Menu;
    public PlayerHitbox PlayerHitbox;
    public GameObject PlayerSpawnPoint;
    public int LevelProgress { get; private set; }

    public bool Paused => Menu != null && Menu.Paused;
    public bool IsInGame { get; private set; }
    public SummaryData CurrentLevelSummary;
    public WorldBound WorldBound;
    public ObjectPooler ObjectPooler;

    public event Action<Level> GameOvered;
    public event Action<int> ProgressMade;

    private SortedList<int, Level> _levels;
    private Level _currentLevel;
    private EnemySpawner _currentSpawner;

    private void Awake()
    {
        if (CurrentGame != null)
        {
            Destroy(gameObject);
            return;
        }

        CurrentGame = this;
        DontDestroyOnLoad(this);

        // Load PlayerData
        PlayerData = PlayerData.LoadJsonData(PublicVars.PlayerDataFile);
        LevelProgress = PlayerData.LevelProgress;

        // Setup spawner
        _currentSpawner = FindObjectOfType<EnemySpawner>();
        _currentSpawner.WaveCleared += HandleLastWaveCleared;
        _currentSpawner.LevelInitialized += HandleLevelInitialized;
        _currentSpawner.EnemySpawned += HandleEnemySpawned;
        _levels = new SortedList<int, Level>();

        // Subscribe to player events
        PlayerHitbox.PlayerDied += HandlePlayerDied;
        PlayerHitbox.PlayerCollectedCoin += HandleCoinCollected;

        // When there is no game in progress,
        // disable menu toggling
        Menu.SetCanHide(false);

        foreach (TextAsset file in LevelFiles)
        {
            Level level = LevelParser.ParseLevelFromFile(file, _currentSpawner.spawnpts.Length, _currentSpawner.enemy.Length);
            _levels.Add(level.Info.ID, level);
        }

        Menu.LevelPage.StartLevel = 0;
        Menu.LevelPage.EndLevel = _levels.Count - 1;
        Menu.LevelPage.Setup();
    }

    public void Continue()
    {
        if (_levels.ContainsKey(LevelProgress))
            LoadLevel(LevelProgress);
        else
            LoadLevel(_levels[_levels.Count - 1]);
    }

    public void LoadLevel(int id) => LoadLevel(_levels[_levels.IndexOfKey(id)]);

    public void LoadLevel(Level level)
    {
        // Initialize level summary, this needs to be done first
        // as our event handlers update it
        CurrentLevelSummary = new SummaryData();

        // Reset the spawner
        _currentSpawner.Reset(level);
        _currentSpawner.Active = true;
        _currentLevel = level;

        // Reset player states
        PlayerHitbox.Player.transform.position = PlayerSpawnPoint.transform.position;
        PlayerHitbox.Player.SetActive(true);
        PlayerHitbox.Dead = false;

        // Navigate through the menu
        Menu.SetCanHide(true);
        Menu.Back();
        Menu.HideMenu();

        // Update IsInGame state
        IsInGame = true;
    }

    // The followings are individual handlers for various GameObject

    private void HandleEnemyDestroyed(EnemyHealth health)
    {
        CurrentLevelSummary.DestructionCount++;
        CurrentLevelSummary.Score += health.MaxHealth;
    }

    private void HandleCoinCollected(GameObject gameObject)
    {
        CurrentLevelSummary.Coin++;
    }

    private void HandlePlayerDied(GameObject player)
    {
        if (GameOvered != null) GameOvered(_currentLevel);
        StartCoroutine(DelayedTask.Wrapper(() =>
        {
            PrepareSummary();
            SaveGame();
            Menu.LockDisplay(Menu.DeathScreen, PublicVars.DEATH_SCREEN_DRUATION);
            Menu.SetCanHide(false);
        }, PublicVars.LOSE_DELAY));
    }

    // The followings are the handlers for EnemySpawner

    private void HandleEnemySpawned(EnemySpawner spawner, EnemyHealth health, EnemyMove move)
    {
        health.Destroyed += HandleEnemyDestroyed;
    }

    private void HandleLevelInitialized(EnemySpawner spawner)
    {
        CurrentLevelSummary.TotalEnemies = spawner.TotalEnemies;
    }

    private void HandleLastWaveCleared(EnemySpawner spawner, int wave, bool isLastWave)
    {
        if (!isLastWave) return;
        StopAllCoroutines();
        int nextLevel = _currentLevel.Info.ID + 1;
        if (nextLevel > LevelProgress)
        {
            PlayerData.LevelProgress = nextLevel;
            LevelProgress = nextLevel;
            ProgressMade(nextLevel);
        }
        CurrentLevelSummary.Cleared = true;
        StartCoroutine(DelayedTask.Wrapper(() =>
        {
            PrepareSummary();
            SaveGame();
            Menu.LockDisplay(Menu.WinScreen, PublicVars.WIN_SCREEN_DRUATION);
            Menu.SetCanHide(false);
            IsInGame = false;
        }, PublicVars.WIN_DELAY));
    }

    // Utilities

    private void SaveGame()
    {
        PlayerData.SaveJsonData(PublicVars.PlayerDataFile);
    }

    private void PrepareSummary()
    {
        Menu.SummaryPage.Setup(CurrentLevelSummary);
        Menu.PushPage(Menu.SummaryPage);
    }

    private void Start()
    {
        WorldBound = new WorldBound();
    }

    private void Update()
    {
        if (!Menu.Paused && IsInGame)
        {
            CurrentLevelSummary.CompletionTime += Time.deltaTime;
        }
    }
}