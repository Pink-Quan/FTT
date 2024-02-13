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
        if(isSeeNam) return;  isSeeNam = true;
        caller.enabled = false;
        player.DisableMoveAndUI();
        player.anim.SetDirection(caller.transform.position - col.transform.position);
        GameManager.instance.dialogueManager.StartDialogue(texts.seeNamDeadBody, InitAfterSeeNamDeadBody);
    }

    private void InitAfterSeeNamDeadBody()
    {
        player.EnableMoveAndUI();
        killerHaveMask.gameObject.SetActive(true);
    }

    public void SeeKiller(Collision2D collision,Collider2D caller)
    {
        caller.enabled = false;
        caller.gameObject.SetActive(false);
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.seeKiller, ComeToKiller);
    }

    private void ComeToKiller()
    {
        Vector3[] paths = new Vector3[1] { killerHaveMask.transform.position + Vector3.right / 1.5f };
        player.Move(paths, player.playerMovement.Speed, TalkWithKiller);
    }

    private void TalkWithKiller()
    {
        killerNoMask.gameObject.SetActive(true);
        killerNoMask.transform.position = killerHaveMask.transform.position;
        killerHaveMask.gameObject.SetActive(false);
        player.anim.SetDirection(killerNoMask.transform.position - player.transform.position);
        killerNoMask.anim.SetDirection(player.transform.position - killerNoMask.transform.position);
        GameManager.instance.dialogueManager.StartDialogue(texts.talkWithKiller, NamAppearFromCave);
    }

    private void NamAppearFromCave()
    {
        Nam.gameObject.SetActive(true);
    }
}
