using System;
using System.Linq;
using UnityEngine;

public class LevelParser
{
    int _maxSpawnPoint = 0;
    int _maxEnemyTypeIndex = 0;
    string _filename;
    string _raw;

    public LevelParser(string filename, string raw)
    {
        _filename = filename;
        _raw = raw;
    }

    public static Level ParseLevelFromFile(TextAsset asset, int spawnptsCount, int enemyTypesCount)
    {
        LevelParser parser = new LevelParser(asset.name, asset.text);
        Level level = parser.ParseLevel();
        if (parser._maxSpawnPoint >= spawnptsCount) throw new ArgumentException($"Not enough spawn points! At least {parser._maxSpawnPoint + 1} spawn points are required for this stage");
        if (parser._maxEnemyTypeIndex >= enemyTypesCount) throw new ArgumentException($"Not enough enemy types! At least {parser._maxEnemyTypeIndex + 1} types of enemy are required for this stage");
        return level;
    }

    public Level ParseLevel()
    {
        string[] levelMeta, rawStages, lines = _raw.Split('\n').Select(line => line.Trim()).ToArray();
        int split = Array.FindIndex(lines, 0, lines.Length, line => line == "END OF META");

        levelMeta = lines.Take(split).ToArray();
        rawStages = lines.Skip(split + 1).ToArray();

        LevelMeta levelInfo = ParseMetaInformation(levelMeta);
        Spawner[] spawners = ParseStages(rawStages);

        return new Level(levelInfo, spawners);
    }

    private LevelMeta ParseMetaInformation(string[] levelMeta)
    {
        int id = -1;
        string name = null;
        float interval = PublicVars.WAVE_DEFAULT_INTERVAL;
        foreach (string line in levelMeta)
        {
            string[] segments = line.Split(':').Select(segment => segment.Trim()).ToArray();
            if (segments.Length != 2)
            {
                throw new ArgumentException($"2 arugments expected, but {segments.Length} arguments are given in {_filename}");
            }

            switch (segments[0])
            {
                case "id":
                    id = Int32.Parse(segments[1]);
                    break;
                case "name":
                    name = segments[1];
                    break;
                case "interval":
                    interval = float.Parse(segments[1]);
                    break;
            }
        }

        if (id == -1 || name == null)
        {
            throw new ArgumentException($"key id and name are expected");
        }
        return new LevelMeta(id, name, interval);
    }

    private Spawner[] ParseStages(string[] rawStages)
    {
        _maxSpawnPoint = 0;
        _maxEnemyTypeIndex = 0;

        Spawner[] parsedStages = rawStages.Select(rawStage =>
        {
            // Format:
            //    0000000          v           1           false         1
            // enemy indexes   direction  spawn index   centralized   duration
            //    0000000        1|2         1             true          -1
            Spawner spawner = new Spawner();

            string[] segments = rawStage.Split();
            if (segments.Length < 5) throw new ParsingException($"5 arguments excepted, but {segments.Length} arguments are given in {_filename}");
            try
            {
                spawner.Enemies = segments[0].Select(enemyChar => Int32.Parse(enemyChar.ToString())).ToArray();
                spawner.SpawnPointIndex = Int32.Parse(segments[2]);
                spawner.Centralized = bool.Parse(segments[3]);
                spawner.Duration = Int32.Parse(segments[4]);
                _maxSpawnPoint = Math.Max(spawner.SpawnPointIndex, _maxSpawnPoint);
                _maxEnemyTypeIndex = Math.Max(spawner.Enemies[spawner.Enemies.Max(e => e)], _maxEnemyTypeIndex);
            }
            catch
            {
                throw new ParsingException($"Failed to parse line \"{rawStage}\" in {_filename}");
            }

            if (segments[1].Length == 1)
            {
                spawner.Direction = segments[1] == "v" ? Vector2.down : Vector2.right;
            }
            else
            {
                float[] components = segments[1].Split('|').Select(component => float.Parse(component)).ToArray();
                spawner.Direction = new Vector2(components[0], components[1]);
            }
            return spawner;
        }).ToArray();
        return parsedStages;
    }
}