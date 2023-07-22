using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ExtendedAnimation;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TextBoard : MonoBehaviour
{
    [SerializeField] private Button outButton;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private UIAnimation board;

    //private InputAction cancelInput;

    //private void Start()
    //{
    //    cancelInput = GameManager.instance.input.UI.Cancel;
    //}
    public void ShowText(string text)
    {
        textUI.text = text;
        board.Show();
    }

    public void ShowText(string text,UnityAction OnClose)
    {
        //cancelInput.performed += ctx => HideBoard();
        onClose = OnClose;
        ShowText(text);
    }

    UnityAction onClose;

    public void HideBoard()
    {
        board.Hide();
        onClose?.Invoke();
        onClose = null;
        //cancelInput.performed -= ctx => HideBoard();
    }

}
