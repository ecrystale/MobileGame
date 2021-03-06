using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    private EnemySpawner _spawner;

    public override void OnInspectorGUI()
    {
        EnemySpawner _spawner = (EnemySpawner)target;

        base.OnInspectorGUI();

        if (_spawner.PreviewLevel == null) return;

        EditorGUI.BeginChangeCheck();

        _spawner.SpawnersPreview = LevelParser.ParseLevelFromFile(_spawner.PreviewLevel, _spawner.spawnpts.Length, _spawner.enemy.Length).Spawners;

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_spawner, "Update spawner file");
        }
    }
}