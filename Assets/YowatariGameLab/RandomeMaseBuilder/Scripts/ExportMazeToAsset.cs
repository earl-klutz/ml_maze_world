using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Maze))]
[ExecuteInEditMode]
public class ExportMazeToAsset : MonoBehaviour
{
    private Maze maze;

    private void OnEnable()
    {
        maze = GetComponent<Maze>();
    }

    public Mesh GetMesh()
    {
        return maze.GetMesh();
    }
}
