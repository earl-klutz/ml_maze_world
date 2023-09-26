using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Maze))]
[ExecuteInEditMode]
public class SecureMazeBuilder : MonoBehaviour
{
    private Maze maze;

    private MazeMapBuilder mazeMapBuilder;
    private SecureMazeMeshBuilder secureMazeMeshBuilder;

    private void OnEnable()
    {
        maze = GetComponent<Maze>();
        if (secureMazeMeshBuilder == null)
        {
            secureMazeMeshBuilder = new SecureMazeMeshBuilder();
        }
        maze.SetMeshBuilder(secureMazeMeshBuilder);
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
