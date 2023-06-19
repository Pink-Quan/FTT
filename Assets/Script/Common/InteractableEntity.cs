using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class InteractableEntity : MonoBehaviour
{
    [SerializeField] private float radius = 1;
    [SerializeField] private string interactName= "interact";
    [SerializeField] private bool isGizmos;
    public UnityEvent OnInteract;
    [SerializeField] private PlayerController player;

    bool isEnter;
    void Update()
    {
        if (player == null) return;
        if ((transform.position - player.transform.position).sqrMagnitude <= radius * radius)
        {
            if(!isEnter)
            {
                OnPlayerEnterZone();
                isEnter = true;
            }
            else
            {
                OnPlayerInZone();
            }
        }
        else if(isEnter)
        {
            isEnter = false;
            OnPlayerLeaveZone();
        }
    }

    protected virtual void OnPlayerEnterZone()
    {
        player.ShowItertactButton(OnInteract,interactName);
    }

    protected virtual void OnPlayerInZone()
    {

    }

    protected virtual void OnPlayerLeaveZone()
    {
        player.HideInteractButton();
    }

    private void OnDrawGizmos()
    {
        if (!isGizmos) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public virtual void OnPlayerInteract()
    {
        player.playerMovement.enabled = false;
        player.buttons.SetActive(false);
    }
    
    public virtual void OnPlayerStopInteract()
    {
        player.playerMovement.enabled = true;
        player.buttons.SetActive(true);
        player.interactButton.gameObject.SetActive(false);
    }
}
