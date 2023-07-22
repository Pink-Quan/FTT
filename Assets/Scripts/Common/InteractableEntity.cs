using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InteractableEntity : MonoBehaviour
{
    [SerializeField] protected PlayerController player;
    [SerializeField] private float radius = 1;
    [SerializeField] private string interactName = "Interact";
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool isGizmos;
    public OnInteractEntity OnInteract;
    [Serializable]
    public class OnInteractEntity:UnityEvent<InteractableEntity> { }

    public UnityEvent onPlayerEnterZone;
    public UnityEvent onPlayerInZone;
    public UnityEvent onPlayerLeaveZone;

    public float Radius
    {
        get { return radius; }
        set { radius = value; }
    }
    public bool IsGizmos
    {
        get { return isGizmos; }
    }

    private void Start()
    {
        if (player == null)
        {
            player = GameManager.instance.player;
        }
    }

    protected bool isEnter;
    protected virtual void Update()
    {
        if (player == null) return;
        if ((transform.position - player.transform.position).sqrMagnitude <= radius * radius)
        {
            if (!isEnter)
            {
                OnPlayerEnterZone();
                isEnter = true;
            }
            else
            {
                OnPlayerInZone();
            }
        }
        else if (isEnter)
        {
            isEnter = false;
            OnPlayerLeaveZone();
        }
    }

    protected virtual void OnPlayerEnterZone()
    {
        //Debug.Log("Player enter zone");
        if (canInteract)
            player.ShowInteractButton(OnInteract, interactName,this);
        onPlayerEnterZone?.Invoke();
    }

    protected virtual void OnPlayerInZone()
    {
        onPlayerInZone?.Invoke();
    }

    protected virtual void OnPlayerLeaveZone()
    {
        if (canInteract)
            player.HideInteractButton();
        onPlayerLeaveZone?.Invoke();
    }

    public virtual void DisableMoveAndUI()
    {
        player.DisableMove();
        player.HideButtons();
    }

    public virtual void EnableMoveAndUI()
    {
        player.ShowButtons();
        player.EnableMove();
        player.interactButton.gameObject.SetActive(false);
    }

    public void ShowInteractButton()
    {
        player.ShowInteractButton();
    }

    public void HideInteractButton()
    {
        player.HideInteractButton();
    }

    private void OnDisable()
    {
        HideInteractButton();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(InteractableEntity), true)]
public class InteractableEntityEditor : Editor
{
    private InteractableEntity obj;
    private void OnEnable()
    {
        obj = (InteractableEntity)target;
    }
    private void OnSceneGUI()
    {
        if (!obj.IsGizmos) return;
        obj.Radius = Handles.RadiusHandle(Quaternion.identity, obj.transform.position, obj.Radius);
        obj.transform.position = Handles.FreeMoveHandle(obj.transform.position, Quaternion.identity, 0.2f, Vector3.one * 0.1f, Handles.DotHandleCap);
    }
}

#endif
