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
    public float RateOfFire = 0.1f;
    public float ShotSpeed = 12f;
    public float ShotSize = 1f;
    public float BulletSpawnOffset = 0.3f;
    public int Damage = 15;
    public int NumberOfBullets = 2;
    public int LevelProgress = 0;

    public bool[] AbilitiesOwned = { false, false, false, false, false };
    public bool[] AbilitiesEnabled = { false, false, false, false, false };
    public int BouncyBulletsLevel = 0;
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