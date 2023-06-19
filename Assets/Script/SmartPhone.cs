using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SmartPhone : MonoBehaviour
{
    public SwitchButton.OnSwitchButton OnTurnFlashLight;
    public Button outButton;
    public GameObject flashLight;
    public void TurnFlashLight(bool isOn)
    {
        flashLight.SetActive(isOn);
        OnTurnFlashLight?.Invoke(isOn);
    }

    private void OnDestroy()
    {
        Destroy(flashLight);
    }
}
