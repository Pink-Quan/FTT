using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FlashLightHidenEntity : InteractableEntity
{
    [SerializeField] private Vector2 checkDirection = Vector2.down;
    protected override void OnPlayerEnterZone()
    {
        if (player.IsFlashLight())
        {
            Vector3 playerDir = player.directionTrasform.up * -1f;
            if (Vector3.Dot(playerDir, checkDirection) >= 0)
                base.OnPlayerEnterZone();
        }
    }

    bool isLight;
    protected override void OnPlayerInZone()
    {
        if (player.IsFlashLight() && !isLight)
        {
            Vector3 playerDir = player.directionTrasform.up * -1f;
            if (Vector3.Dot(playerDir, checkDirection) >= 0)
            {
                base.OnPlayerEnterZone();
                isLight = true;
            }
        }
        base.OnPlayerInZone();
    }

    protected override void OnPlayerLeaveZone()
    {
        isLight = false;
        base.OnPlayerLeaveZone();
    }
}