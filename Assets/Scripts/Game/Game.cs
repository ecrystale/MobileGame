using UnityEngine;

public class Game : MonoBehaviour
{
    static Game CurrentGame;

    public PlayerData PlayerData { get; set; }

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
    }

    public void SaveGame()
    {
        PlayerData.SaveJsonData(PublicVars.PlayerDataFile);
    }
}