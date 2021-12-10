public struct Level
{
    public LevelMeta Info;
    public Spawner[] Spawners;
    public Level(LevelMeta info, Spawner[] spawners)
    {
        Info = info;
        Spawners = spawners;
    }
}