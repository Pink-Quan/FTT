using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveManager : MonoBehaviour
{
    public CharacterController killerHaveMask;
    public CharacterController killerNoMask;
    public CharacterController Nam;
    public PlayerController player;

    [SerializeField] private GameObject seeKiller;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera mainCam;

    private CaveText texts;

    private void Start()
    {
        player.DisableMoveAndUI();
        killerHaveMask.gameObject.SetActive(false);
        Nam.gameObject.SetActive(false);
        texts = Resources.Load<CaveText>($"Texts/Cave/{PlayerPrefs.GetString("Language", "Eng")}");
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Phone", 1, player.inventory);
        player.transform.DORotate(Vector3.zero, 1);
        DOVirtual.DelayedCall(2, () =>
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.firstDialogue, player.EnableMoveAndUI);
        });
    }

    bool didGetInCave;
    public void GetIntoCave()
    {
        if (didGetInCave) return; didGetInCave = true;
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.inCaveDialogue, player.EnableMoveAndUI);
    }

    bool isSeeNam;
    public void SeeNamBody(Collider2D col, Collider2D caller)
    {
        if (!player.IsFlashLight()) return;
        if (isSeeNam) return; isSeeNam = true;
        caller.enabled = false;
        player.DisableMoveAndUI();
        player.anim.SetDirection(caller.transform.position - col.transform.position);
        GameManager.instance.dialogueManager.StartDialogue(texts.seeNamDeadBody, InitAfterSeeNamDeadBody);
    }

    private void InitAfterSeeNamDeadBody()
    {
        player.EnableMoveAndUI();
        killerHaveMask.gameObject.SetActive(true);
        seeKiller.SetActive(true);
    }

    public void SeeKiller(Collision2D collision, Collider2D caller)
    {
        caller.enabled = false;
        caller.gameObject.SetActive(false);
        player.DisableMoveAndUI();
        player.TurnFlashLight(false);

        DOVirtual.DelayedCall(1, () => GameManager.instance.dialogueManager.StartDialogue(texts.seeKiller, ComeToKiller));
    }

    private void ComeToKiller()
    {
        Vector3[] paths = new Vector3[1] { killerHaveMask.transform.position + Vector3.right / 1.2f };
        player.Move(paths, player.playerMovement.Speed, TalkWithKiller);
    }

    private void TalkWithKiller()
    {
        killerNoMask.gameObject.SetActive(true);
        killerNoMask.transform.position = killerHaveMask.transform.position;
        killerHaveMask.gameObject.SetActive(false);
        player.anim.SetDirection(killerNoMask.transform.position - player.transform.position);
        killerNoMask.anim.SetDirection(player.transform.position - killerNoMask.transform.position);
        GameManager.instance.dialogueManager.StartDialogue(texts.talkWithKiller, KillerTalkAboutTheTruth);
    }

    private void KillerTalkAboutTheTruth()
    {
        mainCam.Follow = null;
        DOVirtual.Float(mainCam.m_Lens.OrthographicSize, 2.5f, 1, oSize =>
        {
            mainCam.m_Lens.OrthographicSize = oSize;
        }).OnComplete(() =>
        {
            GameManager.instance.textBoard.ShowText(texts.lazyArtist, () =>
            {
                GameManager.instance.dialogueManager.StartDialogue(texts.killerTalkAboutTheTruth, NamApear);
            });
        });
    }

    private void NamApear()
    {
        Nam.gameObject.SetActive(true);
        mainCam.Follow = Nam.transform;
        Vector3[] paths = new Vector3[1] { killerNoMask.transform.position + Vector3.up / 2 + Vector3.right / 2 };
        Nam.Move(paths, player.playerMovement.Speed, () =>
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.namJoinConversation, KillerPushLinhDown);
        });
    }

    private void KillerPushLinhDown()
    {
        mainCam.Follow = null;
        mainCam.transform.DOMoveY(mainCam.transform.position.y - 10, 1).OnComplete(() =>
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.killerPushLinhDown, PushDownAnim);
        });
    }

    private void PushDownAnim()
    {
        player.transform.DOMoveY(player.transform.position.y - 20, 4);
        player.transform.DORotate(new Vector3(0, 0, 360), 4, RotateMode.LocalAxisAdd);

        killerNoMask.transform.position += Vector3.left;
        killerNoMask.transform.DOMoveY(player.transform.position.y - 20, 4);
        killerNoMask.transform.DORotate(new Vector3(0, 0, -360), 4, RotateMode.LocalAxisAdd);

        DOVirtual.DelayedCall(4, ToHosiptalScene);
    }

    private void ToHosiptalScene()
    {
        print("To Hosiptal scene");
    }
}
