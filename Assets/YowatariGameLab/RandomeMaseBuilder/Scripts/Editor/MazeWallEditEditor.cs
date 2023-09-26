using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeWallEdit))]
public class MazeWallEditEditor : Editor
{
    private string mapText = "";

    public MazeWallEdit mazeWallEdit { get { return (MazeWallEdit)target; } }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        bool isChange = false;
        if (mapText == "")
        {
            buildMapString();
        }

        if (GUILayout.Button("Reload"))
        {
            isChange = true;
        }

        var newFloorChar = EditorGUILayout.DelayedTextField("Floor Character", mazeWallEdit.floorChar);
        if (newFloorChar != mazeWallEdit.floorChar)
        {
            mazeWallEdit.floorChar = newFloorChar;
            isChange = true;
        }

        var newWallChar = EditorGUILayout.DelayedTextField("Wall Character", mazeWallEdit.wallChar);
        if (newWallChar != mazeWallEdit.wallChar)
        {
            mazeWallEdit.wallChar = newWallChar;
            isChange = true;
        }

        var newHoleChar = EditorGUILayout.DelayedTextField("Hole Character", mazeWallEdit.holeChar);
        if (newHoleChar != mazeWallEdit.holeChar)
        {
            mazeWallEdit.holeChar = newHoleChar;
            isChange = true;
        }

        if (isChange)
        {
            buildMapString();
        }
        mapText = EditorGUILayout.TextArea(mapText);

        if (GUILayout.Button("Apply"))
        {
            var replacedMap = mapText
            .Replace(mazeWallEdit.floorChar, "{0}")
            .Replace(mazeWallEdit.wallChar, "{1}")
            .Replace(mazeWallEdit.holeChar, "{2}")
            .Replace("{0}", "0")
            .Replace("{1}", "1")
            .Replace("{2}", "2")
            ;
            var row = new List<uint>();
            var map = new List<List<uint>>();
            int maxWidth = 0;
            foreach (var c in replacedMap)
            {
                switch (c)
                {
                    case '0':
                        row.Add(0);
                        break;
                    case '1':
                        row.Add(1);
                        break;
                    default:
                    case '2':
                        row.Add(2);
                        break;
                    case '\n':
                        maxWidth = Mathf.Max(maxWidth, row.Count);
                        map.Add(row);
                        row = new List<uint>();
                        break;
                }
            }
            if (row.Count > 0)
            {
                maxWidth = Mathf.Max(maxWidth, row.Count);
                map.Add(row);
            }

            for (int y = 0; y < map.Count; y++)
            {
                for (; map[y].Count < maxWidth;)
                {
                    map[y].Add(2);
                }
            }
            mazeWallEdit.SetMap(map);
        }
    }

    private void buildMapString()
    {
        var map = mazeWallEdit.GetMap();
        mapText = "";
        foreach (var row in map)
        {
            foreach (var cell in row)
            {
                switch (cell)
                {
                    case 0:
                        mapText += mazeWallEdit.floorChar;
                        break;
                    case 1:
                        mapText += mazeWallEdit.wallChar;
                        break;
                    case 2:
                        mapText += mazeWallEdit.holeChar;
                        break;
                    default:
                        mapText += "?";
                        break;
                }
            }
            mapText += "\n";
        }
        mapText = mapText.TrimEnd();
    }
}
