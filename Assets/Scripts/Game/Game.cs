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
    public int CurrentLevel = 0;
    public MenuManager Menu;
    public PlayerHitbox PlayerHitbox;
    public int LevelProgress { get; private set; }

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
        LevelProgress = PlayerData.levelProgress;

        // Setup spawner
        _currentSpawner = FindObjectOfType<EnemySpawner>();
        _currentSpawner.WaveCleared += HandleLastWaveCleared;
        _levels = new SortedList<int, Level>();

        // Subscribe to PlayedDied event
        PlayerHitbox.PlayerDied += HandlePlayerDied;

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
        LoadLevel(CurrentLevel);
    }

    public void LoadLevel(int id) => LoadLevel(_levels[_levels.IndexOfKey(id)]);

    public void LoadLevel(Level level)
    {
        // Reset the spawner
        _currentSpawner.Reset(level);
        _currentSpawner.Active = true;
        _currentLevel = level;

        // Reset player states
        PlayerHitbox.Player.SetActive(true);
        PlayerHitbox.Dead = false;

        // Navigate through the menu
        Menu.SetCanHide(true);
        Menu.Back();
        Menu.HideMenu();
    }

    public void HandlePlayerDied(GameObject player)
    {
        if (GameOvered != null) GameOvered(_currentLevel);
        Menu.LockDisplay(Menu.DeathScreen, PublicVars.DEATH_SCREEN_DRUATION);
        Menu.SetCanHide(false);
    }

    public void HandleLastWaveCleared(EnemySpawner spawner, int wave, bool isLastWave)
    {
        if (!isLastWave) return;
        StopAllCoroutines();
        int nextLevel = _currentLevel.Info.ID + 1;
        if (nextLevel > LevelProgress)
        {
            PlayerData.levelProgress = nextLevel;
            LevelProgress = nextLevel;
            ProgressMade(nextLevel);
        }
        SaveGame();
        Menu.LockDisplay(Menu.WinScreen, PublicVars.WIN_SCREEN_DRUATION);
        Menu.SetCanHide(false);
    }

    public void SaveGame()
    {
        PlayerData.SaveJsonData(PublicVars.PlayerDataFile);
    }
}