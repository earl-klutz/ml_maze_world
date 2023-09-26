using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Maze))]
[ExecuteInEditMode]
public class MazeUvEdit : MonoBehaviour
{
    private Maze maze;
    private void OnEnable()
    {
        maze = GetComponent<Maze>();
    }

    public Rect GetUvWall()
    {
        return maze.GetUvWall();
    }
    public void SetUvWall(Rect newUv)
    {
        maze.SetUvWall(newUv);
        maze.buildMesh();
    }
    public Rect GetUvTop()
    {
        return maze.GetUvTop();
    }
    public void SetUvTop(Rect newUv)
    {
        maze.SetUvTop(newUv);
        maze.buildMesh();
    }
    public Rect GetUvFloor()
    {
        return maze.GetUvFloor();
    }
    public void SetUvFloor(Rect newUv)
    {
        maze.SetUvFloor(newUv);
        maze.buildMesh();
    }

    public Texture GetTexture()
    {
        return maze.GetTexture();
    }
}
