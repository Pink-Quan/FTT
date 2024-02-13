using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Language", menuName = "ScriptableObjects/Texts/Cave")]
public class CaveText : ScriptableObject
{
    public Dialogue firstDialogue;
    public Dialogue inCaveDialogue;
    public Dialogue seeNamDeadBody;
    public Dialogue[] seeKiller;
    public Dialogue[] talkWithKiller;
 }
