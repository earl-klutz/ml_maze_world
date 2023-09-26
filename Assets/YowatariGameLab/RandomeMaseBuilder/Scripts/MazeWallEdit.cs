using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Maze))]
[ExecuteInEditMode]
public class MazeWallEdit : MonoBehaviour
{
    private Maze maze;
    [HideInInspector] public string floorChar = "0";
    [HideInInspector] public string wallChar = "1";
    [HideInInspector] public string holeChar = " ";

    private void OnEnable()
    {
        maze = GetComponent<Maze>();
    }

    public List<List<uint>> GetMap()
    {
        return maze.GetMap();
    }
    public void SetMap(List<List<uint>> map)
    {
        maze.SetMap(map);
        maze.buildMesh();
    }
}
