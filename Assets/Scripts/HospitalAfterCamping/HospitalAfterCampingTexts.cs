using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Language", menuName = "ScriptableObjects/Texts/Hospital After Camping")]
public class HospitalAfterCampingTexts : ScriptableObject
{
    public Dialogue[] dialoguesWithNurse;
    public Dialogue[] dialoguesWithMai;
    public Dialogue needToEscape;
    public Dialogue[] stopPlayer;
    public string playerFailedToEscape;
    public Dialogue escapeSuccessfully;
}
