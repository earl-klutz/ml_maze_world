using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(MazeUvEdit))]
public class MazeUvEditEditor : Editor
{
    public MazeUvEdit uvEditMaze { get { return (MazeUvEdit)target; } }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Set Wall UV "))
        {
            var w = EditorWindow.GetWindow<UVEditor>("Wall UV");
            w.SetMazeUvEdit(uvEditMaze, UVEditor.Mode.Wall);
        }
        if (GUILayout.Button("Set Roof UV"))
        {
            var w = EditorWindow.GetWindow<UVEditor>("Roof UV");
            w.SetMazeUvEdit(uvEditMaze, UVEditor.Mode.Top);
        }
        if (GUILayout.Button("Set Floor UV"))
        {
            var w = EditorWindow.GetWindow<UVEditor>("Floor UV");
            w.SetMazeUvEdit(uvEditMaze, UVEditor.Mode.Floor);
        }
    }
}

public class UVEditor : EditorWindow
{
    public enum Mode { Wall, Top, Floor }

    private MazeUvEdit _uvEditMaze;
    private Mode _mode;

    public float uMin = 0;
    public float uMax = 1;
    public float vMin = 0;
    public float vMax = 1;
    private float[] _floatArrayU = new float[2];
    private float[] _floatArrayV = new float[2];
    private static readonly GUIContent[] Contents = new GUIContent[]
        {
        new GUIContent("S"),
        new GUIContent("E"),
        };

    public void SetMazeUvEdit(MazeUvEdit uvEditMaze, Mode mode)
    {
        _uvEditMaze = uvEditMaze;
        _mode = mode;

        Rect uv;
        switch (_mode)
        {
            case Mode.Wall:
                uv = _uvEditMaze.GetUvWall();
                break;
            case Mode.Top:
                uv = _uvEditMaze.GetUvTop();
                break;
            case Mode.Floor:
                uv = _uvEditMaze.GetUvFloor();
                break;
            default:
                return;
        }
        uMin = uv.x;
        uMax = uv.width;
        vMin = 1 - uv.height;
        vMax = 1 - uv.y;
    }

    public void OnGUI()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label("U");
            EditorGUILayout.MinMaxSlider(ref uMin, ref uMax, 0f, 1f);
        }
        _floatArrayU[0] = uMin;
        _floatArrayU[1] = uMax;
        var rect = GUILayoutUtility.GetRect(position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.MultiFloatField(rect, Contents, _floatArrayU);
        uMin = _floatArrayU[0];
        uMax = _floatArrayU[1];

        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label("V");
            EditorGUILayout.MinMaxSlider(ref vMin, ref vMax, 0f, 1f);
        }
        _floatArrayV[0] = vMin;
        _floatArrayV[1] = vMax;
        rect = GUILayoutUtility.GetRect(position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.MultiFloatField(rect, Contents, _floatArrayV);
        vMin = _floatArrayV[0];
        vMax = _floatArrayV[1];

        Texture texture = _uvEditMaze.GetTexture();
        if (texture == null)
        {
            Debug.Log("No texture");
            var sampleTexture = new Texture2D(1, 1);
            sampleTexture.LoadImage(File.ReadAllBytes("Assets/YowatariGames/RandomeMaseBuilder/Materials/SampleTexture.png"));
            texture = sampleTexture;
        }
        texture.filterMode = FilterMode.Point;
        rect = GUILayoutUtility.GetRect(position.width, EditorGUIUtility.singleLineHeight);
        rect.height = rect.width;
        rect.y = 128;
        GUI.DrawTextureWithTexCoords(rect, texture, new Rect(uMin, 1 - vMax, uMax - uMin, (vMax - vMin)));

        if (GUILayout.Button("Save"))
        {
            switch (_mode)
            {
                case Mode.Wall:
                    _uvEditMaze.SetUvWall(new Rect(uMin, 1 - vMax, uMax, 1 - vMin));
                    break;
                case Mode.Top:
                    _uvEditMaze.SetUvTop(new Rect(uMin, 1 - vMax, uMax, 1 - vMin));
                    break;
                case Mode.Floor:
                    _uvEditMaze.SetUvFloor(new Rect(uMin, 1 - vMax, uMax, 1 - vMin));
                    break;
            }
            this.Close();
        }
    }
}
