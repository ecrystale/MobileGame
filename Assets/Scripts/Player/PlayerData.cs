using System.IO;
using UnityEngine;

/// <summary>A serializable class to hold player data</summary>
[System.Serializable]
public class PlayerData
{
    public float RateOfFire = 0.1f;
    public float ShotSpeed = 12f;
    public float ShotSize = 1f;
    public int NumberOfBullets = 1;
    public int LevelProgress = 0;

    public bool HasBoom = false;
    public bool HasMagnet = false;
    public bool BouncyBullets = false;
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