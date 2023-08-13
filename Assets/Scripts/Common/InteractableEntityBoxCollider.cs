using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleBoxCollider))]
public class InteractableEntityBoxCollider : InteractableEntity
{
    [SerializeField] private SimpleBoxCollider col;

    private void Awake()
    {
        if(col == null)
            col = GetComponent<SimpleBoxCollider>();
    }
    protected override void Update()
    {
        if (player == null) return;
        if (col.CheckPointJob(playerTransform.position))
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
