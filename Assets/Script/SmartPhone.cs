using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SmartPhone : MonoBehaviour
{
    public SwitchButton.OnSwitchButton OnTurnFlashLight;

    public void TurnFlashLight(bool isOn)
    {
        OnTurnFlashLight?.Invoke(isOn);
    }
}
