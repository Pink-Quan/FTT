using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapManager : MonoBehaviour
{
    [SerializeField] private CharacterController Ngan;
    [SerializeField] private CharacterController Minh;
    [SerializeField] private CharacterController Mai;
    [SerializeField] private CharacterController Nam;
    [SerializeField] private CharacterController Hung;
    [SerializeField] private PlayerController player;

    [Header("Start Camping")]
    [SerializeField] private Vector3 playerStartCampingPos;
    [SerializeField] private GameObject startCampingInteractableEntities;

    private MainMapTexts texts;

    private void Start()
    {
        AddKeyToPlayer();
        texts = Resources.Load<MainMapTexts>($"Texts/MainMap/{PlayerPrefs.GetString("Language", "Eng")}");
        switch ((GameProgress)PlayerPrefs.GetInt("Progress"))
        {
            case GameProgress.StartCamping:
                InitStartCamping();
                Invoke("FirstComunicateWithManager", 2);
                break;
        }
    }

    private void InitStartCamping()
    {
        player.DisableMove();
        player.HideUI();

        player.transform.position = playerStartCampingPos;
        Ngan.transform.position = playerStartCampingPos + Vector3.up * 2 + Vector3.left * 1.5f;
        Minh.transform.position = playerStartCampingPos + Vector3.up * 2 + Vector3.left / 2;
        Mai.transform.position = playerStartCampingPos + Vector3.left * 1.1f;
        Hung.transform.position = playerStartCampingPos + Vector3.left * 2 * 1.1f;
        Nam.transform.position = playerStartCampingPos + Vector3.right * 1.1f;

        player.anim.SetDirection(Vector3.up);
        Ngan.anim.SetDirection(Vector3.down);
        Minh.anim.SetDirection(Vector3.down);
        Mai.anim.SetDirection(Vector3.up);
        Hung.anim.SetDirection(Vector3.up);
        Nam.anim.SetDirection(Vector3.up);

        startCampingInteractableEntities.gameObject.SetActive(true);
    }

    private void FirstComunicateWithManager()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.theFirstConversationsWithManagers, PlayerStartFirstMission);
    }

    private void PlayerStartFirstMission()
    {
        player.EnableMove();
        player.ShowUI();
    }

    public void AddKeyToPlayer()
    {
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Key", 1, player.inventory);
    }
}
