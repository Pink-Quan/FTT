using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Texts/Come back Linhs Home text", fileName ="Language")]
public class ComeBackLinhsHomeText : ScriptableObject
{
    public Dialogue[] conversationBetweenHelperAndLinh;

    public Dialogue[] aboutToGoHome;
}
