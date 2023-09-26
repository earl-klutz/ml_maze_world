using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SecureMazeHierarchyMenu
{
    [MenuItem("GameObject/3D Object/SecureMaze", false, 0)]
    public static void CreateStandardMaze()
    {
        GameObject secureMaze = new GameObject();
        secureMaze.name = "SecureMaze";

        GameObject activeGameObject = Selection.activeGameObject;
        if (activeGameObject != null)
        {
            secureMaze.transform.SetParent(activeGameObject.transform, false);
        }

        var assets = AssetDatabase.FindAssets("MazeSampleMaterial");
        var path = AssetDatabase.GUIDToAssetPath(assets[0]);
        SecureMazeBuilder smbc = secureMaze.AddComponent<SecureMazeBuilder>();
        smbc.BuildMesh(10, 10);
        secureMaze.GetComponent<Renderer>().material = AssetDatabase.LoadAssetAtPath<Material>(path);
    }
}
