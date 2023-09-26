using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SecureMazeBuilder))]
public class SecureMazeEditor : Editor
{
    int width = 10;
    int height = 10;
    public SecureMazeBuilder secureMaze { get { return (SecureMazeBuilder)target; } }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        width = EditorGUILayout.IntField("Width", width);
        if (width < 0)
        {
            width = 0;
        }
        height = EditorGUILayout.IntField("Height", height);
        if (height < 0)
        {
            height = 0;
        }
        if (GUILayout.Button("Build Mesh"))
        {
            secureMaze.BuildMesh(width, height);
        }
    }
}
