using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorLock : MonoBehaviour
{
    [SerializeField] private Color[] lockColor;
    [SerializeField] private ColorLockButton[] lockButton;
    [SerializeField] private string password;

    public UnityEvent onCorrectPassword;

    private void Awake()
    {
        foreach (var but in lockButton)
        {
            but.Init(lockColor);
        }
    }

    public void EnterPassword()
    {
        for (int i = 0; i < lockButton.Length; i++)
        {
            if (!(lockButton[i].currentIndex + '0' == password[i]))
            {
                return;
            }
        }
        onCorrectPassword?.Invoke();
    }
}
