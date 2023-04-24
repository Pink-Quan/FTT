using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ExtendedAnimation;
using UnityEngine.Events;

public class TextBoard : MonoBehaviour
{
    [SerializeField] private Button outButton;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private UIAnimation board;
    public void ShowText(string text)
    {
        textUI.text = text;
        board.Show();
    }

    public void ShowText(string text,UnityAction OnClose)
    {
        outButton.onClick.RemoveAllListeners();
        outButton.onClick.AddListener(OnClose);
        ShowText(text);
    }

}
