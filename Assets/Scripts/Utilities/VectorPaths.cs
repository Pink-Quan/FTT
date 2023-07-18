using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class VectorPaths : MonoBehaviour
{
    public Vector3[] paths;
}
#if UNITY_EDITOR
[CustomEditor(typeof(VectorPaths))]
public class VectorPathsEditor : Editor
{
    VectorPaths vp;
    private void OnEnable()
    {
        vp = target as VectorPaths;
    }

    private void OnSceneGUI()
    {
        for (int i = 0; i < vp.paths.Length; i++)
        {
            vp.paths[i] = Handles.FreeMoveHandle(vp.paths[i], Quaternion.identity, 0.2f, Vector3.one * 0.1f, Handles.DotHandleCap);
        }

        for (int i = 0; i < vp.paths.Length-1; i++)
        {
            Handles.Label(vp.paths[i] + Vector3.up * 0.5f,i.ToString());
            Handles.DrawLine(vp.paths[i], vp.paths[i + 1]);
        }
        Handles.Label(vp.paths[vp.paths.Length - 1] + Vector3.up * 0.5f, (vp.paths.Length - 1).ToString());
    }
}
#endif
