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

    private Transform player;

    private void Start()
    {
        player = GameManager.instance.player.transform;
    }

    public void GoToArea()
    {
        player.position = toAreaPos;
        toArea.SetActive(true);
        seflArea.SetActive(false);
    }

}
#if UNITY_EDITOR
[CustomEditor(typeof(ToSomeWhere))]
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
        t.toAreaPos = Handles.FreeMoveHandle(t.toAreaPos, Quaternion.identity, 0.08f, Vector3.one, Handles.DotHandleCap);
    }
}
#endif