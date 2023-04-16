using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Hospital conversation",fileName ="Language")]
public class HospitalConversation : ScriptableObject
{
    public Dialogue firstConversation;
    public Dialogue wakeUp;

    [Header("Nurse talking with player when walking up")]
    public Dialogue[] talkingWithPlayerAffterWalkingUp;

    
}
