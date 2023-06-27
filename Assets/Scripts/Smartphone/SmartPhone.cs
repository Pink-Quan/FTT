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

    [HideInInspector]
    public GameObject openningApp;

    public void TurnFlashLight(bool isOn)
    {
        flashLight.SetActive(isOn);
        OnTurnFlashLight?.Invoke(isOn);
    }

    private void OnDestroy()
    {
        Destroy(flashLight);
    }

    public void OutApp()
    {
        if (openningApp == null) return;
        openningApp.SetActive(false);
        openningApp=null;
    }

    public void OpenApp(GameObject app)
    {
        app.SetActive(true);
        openningApp = app;
    }
}
