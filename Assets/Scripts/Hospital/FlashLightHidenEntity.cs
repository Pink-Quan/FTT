using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FlashLightHidenEntity : InteractableEntity
{
    protected override void OnPlayerEnterZone()
    {
        if (player.IsFlashLight())
        {
            float zAngle = player.directionTrasform.localRotation.eulerAngles.z;
            if (zAngle<90&&zAngle>-90)
                base.OnPlayerEnterZone();

        }
    }

    private bool isLight;
    protected override void OnPlayerInZone()
    {    
        if(player.IsFlashLight() && !isLight)
        {
            float zAngle = player.directionTrasform.localRotation.eulerAngles.z;
            if (zAngle < 0 && zAngle > -180)
            {
                base.OnPlayerEnterZone();
                isLight = true;
            }
        }
        base.OnPlayerInZone();
    }
}