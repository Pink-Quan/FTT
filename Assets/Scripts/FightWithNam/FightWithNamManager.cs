using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightWithNamManager : MonoBehaviour
{
    public PlayerController Linh;
    public CharacterController Nam;

    public VectorPaths LinhInPath;
    public VectorPaths LinhOutPath;
    public VectorPaths NamInPath;
    public VectorPaths NamOutPath;

    public Cinemachine.CinemachineVirtualCamera mainCam;

    private FightWithNamTexts texts;

    private void Start()
    {
        texts = Resources.Load<FightWithNamTexts>($"Texts/FightWithNamTexts/{PlayerPrefs.GetString("Language", "Eng")}");
        Linh.DisableMoveAndUI();
        Linh.Move(LinhInPath.paths, 5, () =>
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.linhFirstDialogue, NamApear);
        });
    }

    private void SetCharactersFaceToFace()
    {
        Linh.anim.SetDirection(Nam.transform.position - Linh.transform.position);
        Nam.anim.SetDirection(Linh.transform.position - Nam.transform.position);
    }

    private void NamApear()
    {
        Nam.Move(NamInPath.paths, 5, () =>
        {
            SetCharactersFaceToFace();
            GameManager.instance.dialogueManager.StartDialogue(texts.LinNamCoversation, BattleOfLinhNam);
        });
    }

    private void BattleOfLinhNam()
    {
        //mainCam.Follow = Nam.transform;
        GameManager.instance.textBoard.ShowText(texts.fightingNote, EndFight);
    }

    private void EndFight()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.endFightDialogue, LinhEscape);
    }

    private void LinhEscape()
    {
        Linh.Move(LinhOutPath.paths, 5, () =>
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.namToldLinhToStop, () =>
            {
                Nam.Move(NamOutPath.paths, 5, () =>
                {
                    GameManager.instance.transitions.Transition(1, 1, LinhInCar, InitLinhInCar);
                });
            });
        });
    }

    private void LinhInCar()
    {

    }

    private void InitLinhInCar()
    {

    }
}
