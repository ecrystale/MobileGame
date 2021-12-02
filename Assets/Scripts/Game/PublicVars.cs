using System.IO;
using UnityEngine;

public class PublicVars : MonoBehaviour
{
    // Place to store player data for now
    // Will probably be moved to a separate file later (txt?)
    public static float rateOfFire = 0.1f;
    public static float shotSpeed = 12f;
    public static float shotSize = 1f;

    public static string PlayerDataFile = "data.json";
    public static string GetStagePath(string name) => Path.Combine("Assets/Scripts/Stages", name);
}
