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
    public int CurrentStage = 0;
    public MenuManager Menu;
    public PlayerHitbox PlayerHitbox;

    public event Action<Level> GameOvered;

    private List<Level> _levels;
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
        _levels = new List<Level>();
        PlayerHitbox.PlayerDied += HandlePlayerDied;
        Menu.SetCanHide(false);

        foreach (TextAsset file in LevelFiles)
        {
            _levels.Add(LevelParser.ParseLevelFromFile(file, _currentSpawner.spawnpts.Length, _currentSpawner.enemy.Length));
        }
    }

    public void LoadLevel(int id) => LoadLevel(_levels.Find(level => level.Info.ID == id));

    public void LoadLevel(string name) => LoadLevel(_levels.Find(level => level.Info.Name == name));

    public void LoadLevel(Level level)
    {
        _currentSpawner.Reset(level);
        _currentSpawner.Active = true;
        _currentLevel = level;
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
        //PublicVars.TransitionManager.FadeToScene(Levels[++CurrentStage], 1f);
    }

    public void SaveGame()
    {
        PlayerData.SaveJsonData(PublicVars.PlayerDataFile);
    }
}