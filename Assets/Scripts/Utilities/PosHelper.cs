using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PosHelper : MonoBehaviour
{

}
#if UNITY_EDITOR
[CustomEditor(typeof(PosHelper)),CanEditMultipleObjects]
public class PosHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("To Scene Center"))
            ToCenter();
    }
    private void ToCenter()
    {
        Vector3 pos = SceneView.lastActiveSceneView.pivot;
        foreach (var target in targets)
        {
            ((PosHelper)target).transform.position = pos;
        }
    }
}
#endif
