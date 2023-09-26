using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeMeshBuilder
{
    public Mesh mesh;
    public abstract void buildMesh(List<List<uint>> map, Rect uvWall, Rect uvTop, Rect uvFloor);
    public abstract int Priority();
}
