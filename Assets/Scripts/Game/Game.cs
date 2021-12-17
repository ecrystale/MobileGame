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
    public Upgradable Upgradable(Purchasable purchasable) => Upgradables[((int)purchasable)];
    public Upgradable[] Upgradables;

    public event Action<Level> GameOvered;
    public event Action<int> ProgressMade;
    public event Action<int> CoinsChanged;

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

        // 9 is the number of enums in Purchasable
        Upgradables = new Upgradable[PublicVars.MAX_PURCHASABLES];
        Upgradables[((int)Purchasable.Boom)] = new Upgradable(Purchasable.Boom, "Boom", 100, 15, 1, 50, 0, 2, 3, PlayerData.UpgradesLevels[((int)Purchasable.Boom)], true);
        Upgradables[((int)Purchasable.Split)] = new Upgradable(Purchasable.Split, "Split Shots", 0, 1, 1, 40, 0, 2, 1, PlayerData.UpgradesLevels[((int)Purchasable.Split)], true);
        Upgradables[((int)Purchasable.Bouncy)] = new Upgradable(Purchasable.Bouncy, "Bouncy Shots", 0, 1, 1, 40, 0, 2, 3, PlayerData.UpgradesLevels[((int)Purchasable.Bouncy)], true);
        Upgradables[((int)Purchasable.Homing)] = new Upgradable(Purchasable.Homing, "Homing", 0, 1, 1, 50, 0, 2, 1, PlayerData.UpgradesLevels[((int)Purchasable.Homing)], true);
        Upgradables[((int)Purchasable.Magnet)] = new Upgradable(Purchasable.Magnet, "Magnet", 0, 1, 1, 20, 0, 2, 1, PlayerData.UpgradesLevels[((int)Purchasable.Magnet)], true);
        Upgradables[((int)Purchasable.Damage)] = new Upgradable(Purchasable.Damage, "Damage", 15, 5, 1, 5, 15, 1.5f, 5, PlayerData.UpgradesLevels[((int)Purchasable.Damage)]);
        Upgradables[((int)Purchasable.FireRate)] = new Upgradable(Purchasable.FireRate, "Fire Rate", 0.15f, -0.01f, 0.95f, 20, 10, 1.5f, 5, PlayerData.UpgradesLevels[((int)Purchasable.FireRate)]);
        Upgradables[((int)Purchasable.BulletSpeed)] = new Upgradable(Purchasable.BulletSpeed, "Bullet Speed", 12f, 1, 1, 10, 5, 2, 5, PlayerData.UpgradesLevels[((int)Purchasable.BulletSpeed)]);
        Upgradables[((int)Purchasable.BulletsCount)] = new Upgradable(Purchasable.BulletsCount, "Bullet Count", 2, 1, 1, 50, 10, 2, 5, PlayerData.UpgradesLevels[((int)Purchasable.BulletsCount)]);

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
        Menu.MainPage.Setup();
        Menu.ShopPage.Setup();
    }

    public bool IsLevelValid(int levelID)
    {
        return _levels.ContainsKey(levelID);
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
        CurrentLevelSummary.ID = level.Info.ID;
        CurrentLevelSummary.LevelName = level.Info.Name;

        // Reset the spawner
        _currentSpawner.Reset(level);
        _currentSpawner.Active = true;
        _currentLevel = level;

        // Reset player states
        PlayerHitbox.Player.transform.position = PlayerSpawnPoint.transform.position;
        PlayerHitbox.Player.SetActive(true);
        PlayerHitbox.Dead = false;
        ObjectPooler.CleanAll();

        // Update IsInGame state
        IsInGame = true;

        // Navigate through the menu
        Menu.MainPage.Setup();
        Menu.SetCanHide(true);
        Menu.Back();
        Menu.HideMenu();
    }

    public void HandlePurchase(Purchasable purchasable)
    {
        Upgradable upgradable = Upgradable(purchasable);
        if (PlayerData.Coins < upgradable.CurrentPrice) return;

        PlayerData.Coins -= upgradable.CurrentPrice;
        upgradable.Upgrade();
        PlayerData.UpgradesLevels[((int)purchasable)] = upgradable.Level;
        if (CoinsChanged != null) CoinsChanged(PlayerData.Coins);
        SaveGame();
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
        StopAllCoroutines();
        if (GameOvered != null) GameOvered(_currentLevel);
        StartCoroutine(DelayedTask.Wrapper(() =>
        {
            PrepareSummary();
            PlayerData.Coins += CurrentLevelSummary.Coin;
            SaveGame();
            if (CoinsChanged != null) CoinsChanged(PlayerData.Coins);
            Menu.LockDisplay(Menu.DeathScreen, PublicVars.DEATH_SCREEN_DRUATION);
            Menu.SetCanHide(false);
            IsInGame = false;
            Menu.MainPage.Setup();
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
        PageManager flashScreen = Menu.WinScreen;
        float flashDuration = PublicVars.WIN_SCREEN_DRUATION;
        if (nextLevel > LevelProgress)
        {
            PlayerData.LevelProgress = nextLevel;
            LevelProgress = nextLevel;
            ProgressMade(nextLevel);
            if (!IsLevelValid(LevelProgress))
            {
                flashScreen = Menu.ClearedScreen;
                flashDuration = PublicVars.ALL_CLEARED_SCREEN_DRUATION;
            }
        }
        CurrentLevelSummary.Cleared = true;
        StartCoroutine(DelayedTask.Wrapper(() =>
        {
            PrepareSummary();
            PlayerData.Coins += CurrentLevelSummary.Coin;
            SaveGame();
            if (CoinsChanged != null) CoinsChanged(PlayerData.Coins);
            Menu.LockDisplay(flashScreen, flashDuration);
            Menu.SetCanHide(false);
            IsInGame = false;
            Menu.MainPage.Setup();
        }, PublicVars.WIN_DELAY));
    }

    // Utilities

    public void SaveGame()
    {
        PlayerData.SaveJsonData(PublicVars.PlayerDataFile);
    }

    private void PrepareSummary()
    {
        Menu.Reset();
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