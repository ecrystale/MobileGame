using System.Collections.Generic;
using System.Linq;
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

    private List<Level> _levels;
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
        Menu.Back();
        Menu.HideMenu();
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