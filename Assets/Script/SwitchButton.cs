using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SwitchButton : MonoBehaviour
{
    public bool isOn;
    public Color onColor= Color.gray;

    private Button button;
    private Image image;

    public OnSwitchButton OnClick;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    public void Switch()
    {
        isOn = !isOn;

        if (isOn)
        {
            image.color = onColor;
        }
        else
        {
            image.color = Color.white;
        }

        OnClick?.Invoke(isOn);
    }
    [Serializable]
    public class OnSwitchButton : UnityEvent<bool> { }
}
