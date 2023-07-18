using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Main Menu", fileName = "Language")]
public class MainMenuTexts : ScriptableObject
{
    [TextArea]
    public string information;
    [TextArea]
    public string howToPlay;
}
