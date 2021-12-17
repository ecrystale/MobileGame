public struct LevelMeta
{
    public int ID;
    public string Name;
    public float Interval;
    public float HealthFactor;

    public LevelMeta(int id, string name, float interval, float healthFactor)
    {
        ID = id;
        Name = name;
        Interval = interval;
        HealthFactor = healthFactor;
    }
}