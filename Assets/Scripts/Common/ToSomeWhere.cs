using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[RequireComponent(typeof(InteractableEntityBoxCollider))]
[Tooltip("This component should be child or component of destination1")]
public class ToSomeWhere : MonoBehaviour
{
    [SerializeField] private GameObject seflArea;
    [SerializeField] private GameObject toArea;

    public Vector3 toAreaPos;

    public bool onDraw = true;
    public float moveSize = 0.08f;

    private Transform player;

    private void Start()
    {
        player = GameManager.instance.player.transform;
    }

    public void GoToArea()
    {
        if (seflArea == null || toArea == null) return;
        player.position = toAreaPos;
        toArea.SetActive(true);
        seflArea.SetActive(false);
    }

}
#if UNITY_EDITOR
[CustomEditor(typeof(ToSomeWhere)), CanEditMultipleObjects]
public class ToSomeWhereEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Set ToArea Position to Transform.position"))
        {
            var t = (ToSomeWhere)target;
            t.toAreaPos = t.transform.position;
        }
    }

    private void OnSceneGUI()
    { 
        var t = (ToSomeWhere)target;
        if (!t.onDraw) return;
        Undo.RecordObject(t, "Change To Area Pos");
        Handles.color = Color.yellow;
        t.toAreaPos = Handles.FreeMoveHandle(t.toAreaPos, Quaternion.identity, t.moveSize, Vector3.one, Handles.DotHandleCap);
    }
}
#endif