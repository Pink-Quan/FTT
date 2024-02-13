using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampingDay4 : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;

    [SerializeField] private GameObject mainMap;
    [SerializeField] private GameObject firstFloor;
    [SerializeField] private GameObject secondFloor;
    [SerializeField] private InteractableEntity toMainMap;

    [SerializeField] private Vector3 playerPos1;
    [SerializeField] private Vector3 NganPos1;
    [SerializeField] private Vector3 MaiPos1;
    [SerializeField] private Vector3 HungPos1;
    [SerializeField] private Vector3 NamPos1;
    [SerializeField] private Vector3 MinhPos1;

    [SerializeField] private GameObject[] bloods;
    [SerializeField] private GameObject seeNamEvent;
    [SerializeField] private GameObject HungBlood;
    [SerializeField] private InteractableEntity firstAidKit;
    [SerializeField] private InteractableEntity[] firstAidCharacters;
    [SerializeField] private GameObject openMapButton;
    [SerializeField] private VectorPaths NamOutPath;
    [SerializeField] private Vector3 NamPos2;
    [SerializeField] private GameObject fence;
    [SerializeField] private GameObject talkWithNamOnCliff;
    [SerializeField] private CinemachineVirtualCamera cineCam;

    private MainMapManager mainMapManager;

    private CharacterController Ngan;
    private CharacterController Minh;
    private CharacterController Mai;
    private CharacterController Nam;
    private CharacterController Hung;
    private CharacterController Killer;
    private PlayerController player;

    private CampingDay4Texts texts;
    public void Init(MainMapManager mainMapManager)
    {
        this.mainMapManager = mainMapManager;
        Ngan = mainMapManager.Ngan;
        Minh = mainMapManager.Minh;
        Mai = mainMapManager.Mai;
        Nam = mainMapManager.Nam;
        Hung = mainMapManager.Hung;
        Killer = mainMapManager.Killer;
        player = mainMapManager.player;

        mainMap.SetActive(false);
        firstFloor.SetActive(true);

        texts = Resources.Load<CampingDay4Texts>($"Texts/MainMap/Day4/{PlayerPrefs.GetString("Language", "Eng")}");

        GameManager.instance.transitions.Transition(1, 1, LinhWakeUp, InitDay4);

        foreach (GameObject g in gameObjects)
            g.SetActive(true);

    }

    private void InitDay4()
    {
        //toMainMap.canInteract = false;
        InitCharFirstDay4(Hung, HungPos1, firstFloor.transform, Quaternion.Euler(0, 0, 90));
        InitCharFirstDay4(Minh, MinhPos1, secondFloor.transform, Quaternion.Euler(0, 0, 90));
        InitCharFirstDay4(Ngan, NganPos1, secondFloor.transform, Quaternion.Euler(0, 0, 90));
        InitCharFirstDay4(Mai, MaiPos1, secondFloor.transform, Quaternion.Euler(0, 0, -90));
        Nam.gameObject.SetActive(false);
        Killer.gameObject.SetActive(false);
        foreach (var b in bloods)
            b.SetActive(true);
        player.DisableMoveAndUI();
        player.SetPositon(playerPos1, Vector2.down);
    }

    private void InitCharFirstDay4(CharacterController character, Vector3 pos, Transform parent, Quaternion fallDir)
    {
        character.gameObject.SetActive(true);
        character.SetPositon(pos, Vector2.down);
        character.transform.SetParent(parent, true);
        character.anim.StopAnimation();
        character.transform.rotation = fallDir;
        character.col.enabled = false;
    }

    private void LinhWakeUp()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.playerWakeUpDialogue, player.EnableMoveAndUI);
    }

    public void SeeHung(Collision2D col, Collider2D calller)
    {
        calller.gameObject.SetActive(false);
        player.DisableMoveAndUI();
        player.anim.SetDirection(Vector2.left);
        GameManager.instance.dialogueManager.StartDialogue(texts.playerSeeHung, player.EnableMoveAndUI);
    }

    public void SeeEveryoneElse(Collision2D col, Collider2D calller)
    {
        calller.gameObject.SetActive(false);
        player.DisableMoveAndUI();
        player.anim.SetDirection(Vector2.left);
        GameManager.instance.dialogueManager.StartDialogue(texts.playerSeeEveryoneElse, player.EnableMoveAndUI);
        seeNamEvent.SetActive(true);
        Nam.gameObject.SetActive(true);
        Nam.transform.position = NamPos1;
        Nam.anim.SetDirection(Vector2.left);
        Nam.transform.SetParent(firstFloor.transform, true);
        HungBlood.SetActive(false);
    }

    public void SeeNam(Collision2D col, Collider2D calller)
    {
        calller.gameObject.SetActive(false);
        player.DisableMoveAndUI();
        player.anim.SetDirection(Vector2.left);
        GameManager.instance.dialogueManager.StartDialogue(texts.playerFirstTalkWithNam, player.EnableMoveAndUI);
        firstAidKit.gameObject.SetActive(true);
    }

    bool isGetAidKit;

    public void GetFirstAidKit(InteractableEntity entity)
    {
        entity.gameObject.SetActive(false);
        mainMapManager.PlayParticalEffect(0, player.transform.position);
        isGetAidKit = true;
        foreach (var i in firstAidCharacters)
            i.canInteract = true;
        GameManager.instance.fastNotification.Show(player.transform.position + Vector3.up, texts.getFirstAidKit);
    }

    int aidChar;
    public void FirstAidCharacter(InteractableEntity entity)
    {
        if (!isGetAidKit) return;
        entity.gameObject.SetActive(false);
        mainMapManager.PlayParticalEffect(0, player.transform.position);
        aidChar++;
        if (aidChar == 3)
        {
            Nam.AddConversationToCharacter(texts.talkWithNamAfterFirstAid, OnDoneTalkWithNamAfterFirstAidAllChar,
                SetFaceToFaceWithNam);
        }
    }
    void SetFaceToFaceWithNam()
    {
        Nam.anim.SetDirection(player.transform.position - Nam.transform.position);
        player.anim.SetDirection(Nam.transform.position - player.transform.position);
    }

    private void OnDoneTalkWithNamAfterFirstAidAllChar()
    {
        player.DisableMoveAndUI();
        openMapButton.SetActive(true);
        Nam.col.enabled = false;
        Nam.Move(NamOutPath.paths, 6, InitFollowNam);
        Nam.ClearConversation();
    }

    private void InitFollowNam()
    {
        Nam.transform.SetParent(mainMap.transform, true);
        Nam.SetPositon(NamPos2, Vector2.down);
        Nam.col.enabled = true;
        player.EnableMoveAndUI();
        fence.SetActive(false);
        talkWithNamOnCliff.SetActive(true);
    }

    public void OnReachCliff(Collision2D col, Collider2D caller)
    {
        caller.gameObject.SetActive(false);
        openMapButton.SetActive(false);
        player.DisableMoveAndUI();
        SetFaceToFaceWithNam();
        DOVirtual.Float(cineCam.m_Lens.OrthographicSize, 3, 1, oSize =>
        {
            cineCam.m_Lens.OrthographicSize = oSize;
        }).OnComplete(() =>
        GameManager.instance.dialogueManager.StartDialogue(texts.talkWithLinhInCliff, LookDown));
    }

    private void LookDown()
    {
        cineCam.Follow = null;
        player.anim.SetDirection(Vector2.down);
        cineCam.transform.DOMoveY(cineCam.transform.position.y - 10, 2).OnComplete(() =>
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.LinhShouldKnowTheTruth, PushLinhDown);
        });
    }

    private void PushLinhDown()
    {
        player.col.enabled = false;
        player.transform.DOMoveY(player.transform.position.y - 20 ,4);
        player.transform.DORotate(new Vector3(0, 0, 360), 3, RotateMode.LocalAxisAdd);
        DOVirtual.DelayedCall(2, ChangeToCaveScene);
    }

    private void ChangeToCaveScene()
    {
        PlayerPrefs.SetInt("Progress", (int)GameProgress.Cave);
        mainMapManager.ChangeToCaveScene();
    }

}
