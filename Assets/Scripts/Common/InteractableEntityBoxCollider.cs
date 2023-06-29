using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleBoxCollider))]
public class InteractableEntityBoxCollider : InteractableEntity
{
    [SerializeField] private SimpleBoxCollider col;
    protected override void Update()
    {
        if (player == null) return;
        if (col.CheckPoint(player.transform.position))
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
}
