using UnityEngine;

[CreateAssetMenu(fileName = "Language", menuName = "ScriptableObjects/Texts/Last Chase")]
public class LastChaseTexts : ScriptableObject
{
    public Dialogue[] firstDialogue;
    public Dialogue[] namToldLinhToStop;
    public Dialogue[] lastDialougueWithNam;
    public string mission;
}
