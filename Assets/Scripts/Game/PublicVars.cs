using System.IO;
using UnityEngine;
public enum Purchasable
{
    FireRate = 0, BulletSpeed = 1, Damage = 2, BulletsCount = 3, Boom = 4, Magnet = 5, Homing = 6, Bouncy = 7, Split = 8
}

public class PublicVars : MonoBehaviour
{

    public const float GENERAL_FADE_TIME = 0.5f;
    public const float DEBOUNCE_INTERVAL = 0.2f;
    public const float WAVE_DEFAULT_INTERVAL = 6f;
    public const float WAVE_INIT_INTERVAL = 2f;
    public const float DEATH_SCREEN_DRUATION = 2.6f;
    public const float WIN_SCREEN_DRUATION = 2.2f;
    public const float ALL_CLEARED_SCREEN_DRUATION = 5.2f;
    public const float FRAME_MOVEMENT_SPEED_CAP = 1f;
    public const float WIN_DELAY = 2f;
    public const float LOSE_DELAY = 0.2f;
    public const int MAX_PURCHASABLES = 9;

    public static TransitionManager TransitionManager;
    public static string PlayerDataFile = "data.json";
    public static string GetStagePath(string name) => Path.Combine("Assets/Scripts/Stages", name);
    private static string GetDataPath(string name) => Path.Combine(Application.persistentDataPath, name);
    // 
    // public static string GetStagePath(string name) => Application.persistentDataPath + "/Assets/Scripts/Stages" + name;
}
