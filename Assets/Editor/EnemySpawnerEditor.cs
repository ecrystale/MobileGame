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

        EditorGUI.BeginChangeCheck();

        _spawner._spawners = _spawner.ParseStages();

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_spawner, "Update spawner file");
        }
    }
}