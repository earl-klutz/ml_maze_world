using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecureMazeMeshBuilder : MazeMeshBuilder
{
    enum Face { Wall, Top, Floor, Back, }

    private Rect _uvWall;
    private Rect _uvTop;
    private Rect _uvFloor;

    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();
    private List<int> triangles = new List<int>();

    public SecureMazeMeshBuilder()
    {
        mesh = new Mesh();
        mesh.name = "SecureMazeMesh";
    }

    public override int Priority()
    {
        return 10;
    }

    void begin()
    {
        mesh = new Mesh();
        mesh.name = "SecureMazeMesh";
        vertices.Clear();
        uvs.Clear();
        triangles.Clear();
    }

    void end()
    {
        mesh.Clear();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    void plotFace(Vector3 pos, Face face, Vector3 normal)
    {
        var up = Vector3.up;
        if (normal.y != 0)
        {
            up = Vector3.forward;
        }
        var facePos = pos + normal * 0.5f;
        if (face == Face.Back)
        {
            normal *= -1;
        }
        var crossX = Vector3.Cross(normal, up);
        var crossY = Vector3.Cross(crossX, normal);

        var start = vertices.Count;

        var a = facePos + (-crossX + crossY) * 0.5f;
        var b = facePos + (crossX + crossY) * 0.5f;
        var c = facePos + (-crossX - crossY) * 0.5f;
        var d = facePos + (crossX - crossY) * 0.5f;

        Rect uv;
        switch (face)
        {
            case Face.Back:
            case Face.Wall:
                uv = _uvWall;
                a.y = 2;
                b.y = 2;
                c.y = 0;
                d.y = 0;
                break;
            case Face.Top:
                uv = _uvTop;
                a.y = 2;
                b.y = 2;
                c.y = 2;
                d.y = 2;
                break;
            default:
            case Face.Floor:
                uv = _uvFloor;
                a.y = 0;
                b.y = 0;
                c.y = 0;
                d.y = 0;
                break;
        }

        vertices.Add(a);
        vertices.Add(b);
        vertices.Add(c);
        vertices.Add(d);

        float texel = 1.0f / 1024.0f;
        uvs.Add(new Vector2(uv.x + texel, uv.height - texel));
        uvs.Add(new Vector2(uv.width - texel, uv.height - texel));
        uvs.Add(new Vector2(uv.x + texel, uv.y + texel));
        uvs.Add(new Vector2(uv.width - texel, uv.y + texel));

        triangles.Add(start + 0);
        triangles.Add(start + 1);
        triangles.Add(start + 2);
        triangles.Add(start + 2);
        triangles.Add(start + 1);
        triangles.Add(start + 3);
    }

    public override void buildMesh(List<List<uint>> map, Rect uvWall, Rect uvTop, Rect uvFloor)
    {
        begin();

        _uvWall = uvWall;
        _uvTop = uvTop;
        _uvFloor = uvFloor;

        var width = map[0].Count;
        var height = map.Count;

        var center = new Vector2(width * 0.5f, height * 0.5f);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var cell = map[y][x];
                if (cell == 0)
                {
                    var pos = new Vector3(-center.x + x, 0, center.y - y);
                    plotFace(pos, Face.Floor, Vector3.up);
                }
                else if (cell == 1)
                {
                    var pos = new Vector3(-center.x + x, 0, center.y - y);
                    plotFace(pos, Face.Top, Vector3.up);
                    int unPlotFaceCount = 0;
                    Vector3 unPlotDir = Vector3.zero;
                    if (y == 0 || map[y - 1][x] != 1)
                    {
                        plotFace(pos, Face.Wall, Vector3.forward);
                    }
                    else
                    {
                        unPlotFaceCount++;
                        unPlotDir = Vector3.forward;
                    }
                    if (x == 0 || map[y][x - 1] != 1)
                    {
                        plotFace(pos, Face.Wall, Vector3.left);
                    }
                    else
                    {
                        unPlotFaceCount++;
                        unPlotDir = Vector3.left;
                    }
                    if (x == width - 1 || map[y][x + 1] != 1)
                    {
                        plotFace(pos, Face.Wall, Vector3.right);
                    }
                    else
                    {
                        unPlotFaceCount++;
                        unPlotDir = Vector3.right;
                    }
                    if (y == height - 1 || map[y + 1][x] != 1)
                    {
                        plotFace(pos, Face.Wall, Vector3.back);
                    }
                    else
                    {
                        unPlotFaceCount++;
                        unPlotDir = Vector3.back;
                    }
                    var isEdge = unPlotFaceCount == 1;
                    if ((isEdge && unPlotDir != Vector3.forward) || !isEdge)
                    {
                        plotFace(pos, Face.Back, Vector3.back);
                    }
                    if ((isEdge && unPlotDir != Vector3.left) || !isEdge)
                    {
                        plotFace(pos, Face.Back, Vector3.right);
                    }
                    if ((isEdge && unPlotDir != Vector3.right) || !isEdge)
                    {
                        plotFace(pos, Face.Back, Vector3.left);
                    }
                    if ((isEdge && unPlotDir != Vector3.back) || !isEdge)
                    {
                        plotFace(pos, Face.Back, Vector3.forward);
                    }
                }
            }
        }
        end();
    }
}
