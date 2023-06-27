using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/In Linh house texts", fileName = "Language")]
public class LinhHouseTexts : ScriptableObject
{
    public Dialogue firstSeftDialogue;

    [TextArea]
    public string getCookBookGuide;
    [TextArea]
    public string getStick;

    public Dialogue seftDialogueAbountCookBook;
    public Dialogue monologueAbountLocker;
    public Dialogue monologueAbountStick;
    public Dialogue thereSomthingUnderTheChair;
}
