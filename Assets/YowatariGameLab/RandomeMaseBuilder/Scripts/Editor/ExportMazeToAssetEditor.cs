using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ExportMazeToAsset))]
public class ExportMazeToAssetEditor : Editor
{
    public ExportMazeToAsset exportMaze { get { return (ExportMazeToAsset)target; } }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Export Maze Asset"))
        {
            {
                var mesh = exportMaze.GetMesh();
                if (mesh == null)
                {
                    Debug.Log("null");
                    return;
                }

                var path = string.Format("Assets/{0}.asset", mesh.name);
                var uniqueFileName = AssetDatabase.GenerateUniqueAssetPath(path);
                var start = uniqueFileName.LastIndexOf('/');
                var end = uniqueFileName.LastIndexOf('.') - start;
                var newName = uniqueFileName.Substring(start, end);
                var beforeName = mesh.name;
                mesh.name = newName;
                try
                {
                    AssetDatabase.CreateAsset(mesh, uniqueFileName);
                    AssetDatabase.SaveAssets();
                }
                catch (UnityException)
                {
                    mesh.name = beforeName;
                    EditorUtility.DisplayDialog("Error", "すでに出力済みの可能性があります", "OK");
                }
            }
        }
    }
}
