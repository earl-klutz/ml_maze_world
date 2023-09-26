using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StandardMazeHierarchyMenu
{
    [MenuItem("GameObject/3D Object/StandardMaze", false, 0)]
    public static void CreateStandardMaze()
    {
        GameObject standardMaze = new GameObject();
        standardMaze.name = "StandardMaze";

        GameObject activeGameObject = Selection.activeGameObject;
        if (activeGameObject != null)
        {
            standardMaze.transform.SetParent(activeGameObject.transform, false);
        }

        var assets = AssetDatabase.FindAssets("MazeSampleMaterial");
        var path = AssetDatabase.GUIDToAssetPath(assets[0]);
        StandardMazeBuilder smbc = standardMaze.AddComponent<StandardMazeBuilder>();
        smbc.BuildMesh(10, 10);
        standardMaze.GetComponent<Renderer>().material = AssetDatabase.LoadAssetAtPath<Material>(path);
    }
}
