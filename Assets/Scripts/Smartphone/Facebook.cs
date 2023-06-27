using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Facebook : MonoBehaviour
{
    [SerializeField] private SmartPhone phone;

    [SerializeField] private TMP_InputField accountInput;
    [SerializeField] private TMP_InputField passwordInput;

    [SerializeField] private string account;
    [SerializeField] private string password;

    [SerializeField] private Button facebookIcon;
    [SerializeField] private GameObject facebookApp;

    public UnityEvent onLoginSuccess;

    private void Start()
    {
        if (facebookIcon != null) facebookIcon.onClick.AddListener(OpenFacebook);
    }

    public void Login()
    {
        if (string.Compare(account, accountInput.text) != 0) return;
        if (string.Compare(password, passwordInput.text) != 0) return;

        onLoginSuccess?.Invoke();
    }

    public void DiableOpenFacebook()
    {
        facebookIcon.onClick.RemoveAllListeners();
    }

    public void OpenFacebook()
    {
        phone.OpenApp(facebookApp);
    }

}
