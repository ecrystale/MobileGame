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

    public event Action<Level> GameOvered;

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
        PlayerData = PlayerData.LoadJsonData(PublicVars.PlayerDataFile);
        _currentSpawner = FindObjectOfType<EnemySpawner>();
        _currentSpawner.WaveCleared += HandleLastWaveCleared;
        _levels = new SortedList<int, Level>();
        PlayerHitbox.PlayerDied += HandlePlayerDied;
        Menu.SetCanHide(false);
        CurrentLevel = PlayerData.level;

        foreach (TextAsset file in LevelFiles)
        {
            Level level = LevelParser.ParseLevelFromFile(file, _currentSpawner.spawnpts.Length, _currentSpawner.enemy.Length);
            _levels.Add(level.Info.ID, level);
        }
    }

    public void Continue()
    {
        LoadLevel(CurrentLevel);
    }

    public void LoadLevel(int id) => LoadLevel(_levels[_levels.IndexOfKey(id)]);

    public void LoadLevel(Level level)
    {
        _currentSpawner.Reset(level);
        _currentSpawner.Active = true;
        _currentLevel = level;
        CurrentLevel = level.Info.ID;
        PlayerHitbox.Player.SetActive(true);
        PlayerHitbox.Dead = false;
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
        PlayerData.level = CurrentLevel;
        SaveGame();
    }

    public void SaveGame()
    {
        PlayerData.SaveJsonData(PublicVars.PlayerDataFile);
    }
}