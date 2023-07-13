using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InformationBoard : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private TMP_Text text;
    private void OnEnable()
    {
        text.text = mainMenu.texts.information;
    }
}
