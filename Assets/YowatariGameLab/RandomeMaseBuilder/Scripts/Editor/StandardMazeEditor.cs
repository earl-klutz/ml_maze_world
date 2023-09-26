using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StandardMazeBuilder))]
public class StandardMazeEditor : Editor
{
    const int LIMIT_LENGTH = 43;

    int width = 10;
    int height = 10;
    public StandardMazeBuilder standardMaze { get { return (StandardMazeBuilder)target; } }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        width = EditorGUILayout.IntSlider("Width", width, 0, LIMIT_LENGTH);
        height = EditorGUILayout.IntSlider("Height", height, 0, LIMIT_LENGTH);
        if (GUILayout.Button("Build Mesh"))
        {
            if (width > LIMIT_LENGTH || height > LIMIT_LENGTH)
            {
                EditorUtility.DisplayDialog("Size Error", "Please make the size " + LIMIT_LENGTH.ToString() + " or less", "OK");
            }
            else
            {
                standardMaze.BuildMesh(width, height);
            }
        }
    }
}
