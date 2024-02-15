using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightWithNamManager : MonoBehaviour
{
    public PlayerController Linh;
    public CharacterController Nam;
    public Cinemachine.CinemachineVirtualCamera mainCam;

    public VectorPaths LinhInPath;
    public VectorPaths LinhOutPath;
    public VectorPaths NamInPath;
    public VectorPaths NamOutPath;

    [SerializeField] private GameObject street;
    [SerializeField] private GameObject streetToCamp;

    [SerializeField] private Vector3 NamOnCarPos = new Vector3(21.08f, -8.52f);

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
        GameManager.instance.dialogueManager.StartDialogue(texts.NamOnCarTellLinhStop, EndScene);
    }

    private void InitLinhInCar()
    {
        Linh.gameObject.SetActive(false);
        street.SetActive(false);
        streetToCamp.SetActive(true);
        Nam.anim.StopAnimation();
        Nam.anim.SpriteRenderer.sprite = Nam.anim.charaterSprites[2];
        Nam.transform.rotation = Quaternion.Euler(0, 0, 90);
        Nam.anim.SpriteRenderer.sortingOrder = 19;
        Nam.transform.position = NamOnCarPos;
    }

    private void EndScene()
    {
        PlayerPrefs.SetInt("Progress", (int)GameProgress.ChaseLinh);
        GameManager.instance.dbManager.UpdateDB();
        GameManager.instance.transitions.Transition(1, 3, null, () =>
        {
            GameManager.instance.soundManager.PlaySound("Car Crash");
            SceneManager.LoadScene("");
        });
    }
}
