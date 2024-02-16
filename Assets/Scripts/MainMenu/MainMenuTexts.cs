using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Texts/Main Menu", fileName = "Language")]
public class MainMenuTexts : ScriptableObject
{
    [TextArea]
    public string information;
    [TextArea]
    public string howToPlay;
    public string faintInHospital;
    public string diedAtCampsite;
    public string arrested;
    public string foundTheTruth;
}
