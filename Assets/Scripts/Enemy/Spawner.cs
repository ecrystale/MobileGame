using UnityEngine;

[System.Serializable]
public struct Spawner
{
    public int SpawnPointIndex;
    /// <value>Value of indexes representing the prefab of the enemies</value>
    public int[] Enemies;
    public Vector2 Direction;
    /// <value>The offset BETWEEN enemies</value>
    public float OffsetBetween;
    public bool Centralized;
    /// <value>The time this group of enemies shall stay</value>
    public float Duration;
    /// <value>The coin reward per enemy</value>
    public int CoinReward;
}