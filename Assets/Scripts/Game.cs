using UnityEngine;

public class Game : MonoBehaviour
{
    private void Awake()
    {
        PlayerData playerData = PlayerData.LoadJsonData(PublicVars.PlayerDataFile);
        playerData.shotSize = 200f;
        playerData.SaveJsonData(PublicVars.PlayerDataFile);
    }
}