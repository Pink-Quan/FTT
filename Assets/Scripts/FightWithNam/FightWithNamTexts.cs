using UnityEngine;

[CreateAssetMenu(fileName = "Language", menuName = "ScriptableObjects/Texts/Fight With Nam Texts")]
public class FightWithNamTexts : ScriptableObject
{
    public Dialogue[] linhFirstDialogue;
    public Dialogue[] namFirsetDialogoe;
    public Dialogue[] LinNamCoversation;
    [TextArea]
    public string fightingNote;
    public Dialogue[] endFightDialogue;
    public Dialogue[] namToldLinhToStop;
}
