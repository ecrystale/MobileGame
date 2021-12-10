using System.IO;
using UnityEngine;

/// <summary>A serializable class to hold player data</summary>
[System.Serializable]
public class PlayerData
{
    public float rateOfFire = 0.1f;
    public float shotSpeed = 12f;
    public float shotSize = 1f;
    public int level = 0;

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