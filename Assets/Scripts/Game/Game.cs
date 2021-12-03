using UnityEngine;

[RequireComponent(typeof(TransitionManager))]
public class Game : MonoBehaviour
{
    static Game CurrentGame;

    public PlayerData PlayerData { get; set; }
    public string[] Stages = { "Main", "ModifyPatterns" };
    public int CurrentStage = 0;


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
        FindObjectOfType<EnemySpawner>().WaveCleared += HandleLastWaveCleared;
    }

    public void HandleLastWaveCleared(EnemySpawner spawner, int wave, bool isLastWave)
    {
        if (!isLastWave) return;
        Debug.Log("stage cleared");
        StopAllCoroutines();
        PublicVars.TransitionManager.FadeToScene(Stages[++CurrentStage], 1f);
    }

    public void SaveGame()
    {
        PlayerData.SaveJsonData(PublicVars.PlayerDataFile);
    }
}