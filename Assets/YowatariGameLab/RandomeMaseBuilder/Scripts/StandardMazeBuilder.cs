using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Maze))]
[ExecuteInEditMode]
public class StandardMazeBuilder : MonoBehaviour
{
    private Maze maze;

    private MazeMapBuilder mazeMapBuilder;
    private StandardMazeMeshBuilder standardMazeMeshBuilder;

    private void OnEnable()
    {
        maze = GetComponent<Maze>();
        if (standardMazeMeshBuilder == null)
        {
            standardMazeMeshBuilder = new StandardMazeMeshBuilder();
        }
        maze.SetMeshBuilder(standardMazeMeshBuilder);
    }

    public void BuildMesh(int width, int height)
    {
        if (mazeMapBuilder == null)
        {
            mazeMapBuilder = new MazeMapBuilder();
        }
        mazeMapBuilder.buildMaze((int)width, (int)height);

        maze.SetMap(mazeMapBuilder.map);
        maze.buildMesh();
    }
}
