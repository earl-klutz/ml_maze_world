using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[ExecuteInEditMode]
public class Maze : MonoBehaviour
{

    private MeshRenderer meshRenderer;
    private MeshFilter meshFiler;
    private MeshCollider meshCollider;

    private MazeMeshBuilder mazeMeshBuilder;

    [HideInInspector] public Rect uvWall = new Rect(0, 0, 0.5f, 1);
    [HideInInspector] public Rect uvRoof = new Rect(0.5f, 0.5f, 1, 1);
    [HideInInspector] public Rect uvFloor = new Rect(0.5f, 0, 1, 0.5f);

    [HideInInspector] public int width = 0;
    [HideInInspector] public int height = 0;
    [HideInInspector] public List<uint> map = new List<uint>();

    private Mesh mesh = null;
    private int meshInstanceId = 0;

    private void OnEnable()
    {
        meshFiler = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    void OnValidate()
    {
        if (mesh != null && mesh.GetInstanceID() != meshInstanceId)
        { SetMesh(mesh); }
    }

    public void SetMeshBuilder(MazeMeshBuilder builder)
    {
        if (mazeMeshBuilder == null || mazeMeshBuilder.Priority() <= builder.Priority())
        {
            mazeMeshBuilder = builder;
        }
    }

    public Rect GetUvWall()
    {
        return uvWall;
    }
    public void SetUvWall(Rect newUv)
    {
        uvWall = newUv;
    }
    public Rect GetUvTop()
    {
        return uvRoof;
    }
    public void SetUvTop(Rect newUv)
    {
        uvRoof = newUv;
    }
    public Rect GetUvFloor()
    {
        return uvFloor;
    }
    public void SetUvFloor(Rect newUv)
    {
        uvFloor = newUv;
    }

    public List<List<uint>> GetMap()
    {
        var mapArray = new List<List<uint>>(height);
        for (int i = 0; i < height; i++)
        {
            mapArray.Add(map.GetRange(i * width, width));
        }
        return mapArray;
    }
    public void SetMap(List<List<uint>> newMap)
    {
        width = newMap[0].Count;
        height = newMap.Count;
        var size = width * height;
        map = new List<uint>(size);
        for (int i = 0; i < height; i++)
        {
            map.AddRange(newMap[i]);
        }
    }

    public Texture GetTexture()
    {
        var mat = GetComponent<Renderer>().sharedMaterial;
        if (mat == null) { return null; }
        foreach (var a in mat.GetTexturePropertyNames())
        {
            var tex = mat.GetTexture(a);
            if (tex)
                return tex;
        }
        return null;
    }

    public void buildMesh()
    {
        mazeMeshBuilder.buildMesh(GetMap(), uvWall, uvRoof, uvFloor);
        SetMesh(mazeMeshBuilder.mesh);
    }

    public void SetMesh(Mesh newMesh)
    {
        mesh = null;
        meshFiler.sharedMesh = null;
        meshCollider.sharedMesh = null;

        mesh = newMesh;
        meshInstanceId = mesh.GetInstanceID();
        meshFiler.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    public Mesh GetMesh()
    {
        if (mesh == null) { buildMesh(); }
        return mesh;
    }
}
