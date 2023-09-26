using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMapBuilder
{
    enum Stuff : uint { None, Wall, }

    private System.Random r = new System.Random();
    public List<List<uint>> map;

    Vector2Int intToDir(int dir)
    {
        switch (dir % 4)
        {
            case 0:
                return Vector2Int.up;
            case 1:
                return Vector2Int.right;
            case 2:
                return Vector2Int.down;
            default:
                return Vector2Int.left;
        }
    }

    public MazeMapBuilder() { }

    public List<List<uint>> buildMaze(int width, int height)
    {
        var fullWidth = ((int)width * 2) + 3;
        var fullHeight = ((int)height * 2) + 3;

        initMap(fullWidth, fullHeight);

        if (width == 0 || height == 0) { return map; }

        var suggestionCnt = width * height;
        List<Vector2Int> suggestion = new List<Vector2Int>((int)suggestionCnt);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var idx = y * width + x;
                suggestion.Add(new Vector2Int(x, y));
            }
        }

        while (suggestion.Count > 0)
        {
            var index = new System.Random(r.Next() * 3).Next(0, suggestion.Count);
            var sug = (suggestion[index] + Vector2Int.one) * 2;
            suggestion.RemoveAt(index);
            if (isNone(sug))
            {
                drawOneWall(sug);
            }
        }

        return map;
    }

    void initMap(int width, int height)
    {
        if (map == null || map.Count != height || map[0].Count != width)
        {
            map = null;
            map = new List<List<uint>>(height);
            for (int i = 0; i < height; i++)
            {
                map.Add(new List<uint>(new uint[width]));
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (y == 0 || y == height - 1 || x == 0 || x == width - 1)
                {
                    map[y][x] = (uint)Stuff.Wall;
                }
                else
                {
                    map[y][x] = (uint)Stuff.None;
                }
            }
        }
    }


    bool isStuff(Vector2Int pos, Stuff stuff)
    {
        return map[pos.y][pos.x] == (uint)stuff;
    }

    bool isNone(Vector2Int pos)
    {
        return isStuff(pos, Stuff.None);
    }

    bool isWall(Vector2Int pos)
    {
        return isStuff(pos, Stuff.Wall);
    }

    void plotStuff(Vector2Int pos, Stuff stuff)
    {
        map[pos.y][pos.x] = (uint)stuff;
    }

    void plotWall(Vector2Int pos)
    {
        plotStuff(pos, Stuff.Wall);
    }

    void plotWallForStack(Stack<Vector2Int> stack)
    {
        foreach (var pos in stack)
        {
            plotWall(pos);
        }
    }

    void drawOneWall(Vector2Int start)
    {
        var cursor = start;
        var route = new Stack<Vector2Int>();
        route.Push(cursor);
        var dir = intToDir(new System.Random(r.Next() * 3).Next(0, 4));
        var blackList = new HashSet<Vector2Int>();
        while (isNone(cursor + dir * 2))
        {
            bool canUp = (!route.Contains(cursor + Vector2Int.up * 2)) && (!blackList.Contains(cursor + Vector2Int.up * 2));
            bool canDown = (!route.Contains(cursor + Vector2Int.down * 2)) && (!blackList.Contains(cursor + Vector2Int.down * 2));
            bool canLeft = (!route.Contains(cursor + Vector2Int.left * 2)) && (!blackList.Contains(cursor + Vector2Int.left * 2));
            bool canRight = (!route.Contains(cursor + Vector2Int.right * 2)) && (!blackList.Contains(cursor + Vector2Int.right * 2));
            if (!canUp && !canDown && !canLeft && !canRight)
            {
                blackList.Add(cursor);
                route.Pop();
                route.Pop();
                cursor = route.Peek();
            }
            else
            {
                var target = cursor + dir * 2;
                if (!route.Contains(target) && !blackList.Contains(target))
                {
                    route.Push(cursor + dir);
                    route.Push(target);
                    cursor = target;
                }
                else if (route.Count == 0)
                {
                    showOnConsole(map, route);
                    return;
                }
            }
            dir = intToDir(new System.Random(r.Next() * 3).Next(0, 4));
        }
        route.Push(cursor + dir);
        plotWallForStack(route);
    }

    static void showOnConsole(List<List<uint>> map, Stack<Vector2Int> s)
    {
        var m = map;
        foreach (var a in s)
        {
            m[a.y][a.x] = 3;
        }
        string str = "";
        for (int y = 0; y < m.Count; y++)
        {
            for (int x = 0; x < m[0].Count; x++)
            {
                switch ((Stuff)m[y][x])
                {
                    case Stuff.None:
                        str += "□";
                        break;
                    case Stuff.Wall:
                        str += "■";
                        break;
                    default:
                        str += "？";
                        break;
                }
            }
            str += "\n";
        }

        Debug.Log(str);
    }
}
