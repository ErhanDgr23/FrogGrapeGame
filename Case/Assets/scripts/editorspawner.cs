using gamefrogs;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Cell))]
public class editorspawner : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Cell cellsc = (Cell)target;

        if(GUILayout.Button("Create Frog"))
            cellsc.spawnchild("frog");

        if (GUILayout.Button("Create Grape"))
            cellsc.spawnchild("grape");

        if (GUILayout.Button("Create Rotater"))
            cellsc.spawnchild("rotater");

        if (GUILayout.Button("Clear Childs"))
            cellsc.spawnchild("child");
    }
}
