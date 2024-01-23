using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class VectorPaths : MonoBehaviour
{
    public Vector3[] paths;

    public bool isDraw;
    public bool isClosed;
    public float lineThickness = 1;
    public Color color = Color.red;

    public void PushBackPoint(Vector3 point)
    {
        var tList = new List<Vector3>(paths);
        tList.Add(point);
        paths = tList.ToArray();

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(VectorPaths)),CanEditMultipleObjects]
public class VectorPathsEditor : Editor
{
    private VectorPaths wp;
    private void OnEnable()
    {
        wp = (VectorPaths)target;
    }
    private void OnSceneGUI()
    {
        if (!wp.isDraw) return;

        Undo.RecordObject(wp, "Change Path");
        Handles.color = wp.color;
        for (int i = 0; i < wp.paths.Length; i++)
        {
            Handles.Label(wp.paths[i] + Vector3.up * 0.5f, i.ToString());

            if (i != wp.paths.Length - 1) Handles.DrawLine(wp.paths[i], wp.paths[i + 1], wp.lineThickness);
            else if (wp.isClosed) Handles.DrawLine(wp.paths[i], wp.paths[(i + 1) % wp.paths.Length], wp.lineThickness);

            wp.paths[i] = Handles.FreeMoveHandle(wp.paths[i], Quaternion.identity, 0.2f, Vector3.one * 0.1f, Handles.DotHandleCap);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Paths to center of Scene"))
        {
            PathsToCenter();
        }
    }

    private void PathsToCenter()
    {
        foreach(var item in targets)
        {
            var path = item as VectorPaths;
            for (int i = 0; i < path.paths.Length; i++)
            {
                path.paths[i] = SceneView.lastActiveSceneView.pivot;
            }
        }
    }
}
#endif