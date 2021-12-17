using System.IO;
using UnityEngine;

public enum Ability
{
    Boom = 0, Magnet = 1, Homing = 2, Bouncy = 3, Split = 4
}

/// <summary>A serializable class to hold player data</summary>
[System.Serializable]
public class PlayerData
{
    private Game _g => Game.CurrentGame;
    public float RateOfFire => _g.Upgradable(Purchasable.FireRate).CurrentPower;
    public float ShotSpeed => _g.Upgradable(Purchasable.BulletSpeed).CurrentPower;
    public int Damage => _g.Upgradable(Purchasable.Damage).CurrentPowerInt;
    public int NumberOfBullets => _g.Upgradable(Purchasable.BulletsCount).CurrentPowerInt;
    public bool Own(int index) => index > 0 && index < UpgradesLevels.Length && UpgradesLevels[index] > 0;

    public int Coins = 0;
    public float ShotSize = 1f;
    public float BulletSpawnOffset = 0.3f;
    public int LevelProgress = 0;

    public int[] UpgradesLevels = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public bool[] AbilitiesEnabled = { false, false, false, false, false };
    public bool SplittingBullets = false;

    private static string GetDataPath(string filename)
    {
        return Path.Combine(Application.persistentDataPath, filename);
    }

    public static PlayerData LoadJsonData(string filename)
    {
        string path = GetDataPath(filename);
        if (!File.Exists(path)) return new PlayerData();

        string data = File.ReadAllText(path);
        return JsonUtility.FromJson<PlayerData>(data);
    }

    public void SaveJsonData(string filename)
    {
        string path = GetDataPath(filename);
        string data = JsonUtility.ToJson(this);
        File.WriteAllText(path, data);
    }
}