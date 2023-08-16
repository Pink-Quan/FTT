using System;
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

    [SerializeField] private ParticleSystem[] effs;
    [SerializeField] private StartCamping startCamping;

    private MainMapTexts texts;

    private void Start()
    {
        texts = Resources.Load<MainMapTexts>($"Texts/MainMap/{PlayerPrefs.GetString("Language", "Eng")}");
        switch ((GameProgress)PlayerPrefs.GetInt("Progress"))
        {
            case GameProgress.StartCamping:
                startCamping.Init(Ngan, Minh, Mai, Nam, Hung, player, texts, this);
                break;
        }
    }

    public void AddConversationToCharacter(CharacterController character, Dialogue dialogue, Action onDone = null)
    {
        var onInteract = character.interact.onInteract;
        character.interact.canInteract = true;
        onInteract.RemoveAllListeners();
        onInteract.AddListener(TalkWithCharacter);

        void TalkWithCharacter(InteractableEntity entity)
        {
            DisablePlayerMoveAndUI();
            GameManager.instance.dialogueManager.StartDialogue(dialogue, DoneTalkWithCharacter);

            void DoneTalkWithCharacter()
            {
                EnablePlayerMoveAndUI();
                player.ShowInteractButton();
            }

            onDone?.Invoke();
        }
    }
    public void AddConversationToCharacter(CharacterController character, Dialogue[] dialogue, Action onDone = null)
    {
        var onInteract = character.interact.onInteract;
        character.interact.canInteract = true;
        onInteract.RemoveAllListeners();
        onInteract.AddListener(TalkWithCharacter);

        void TalkWithCharacter(InteractableEntity entity)
        {
            DisablePlayerMoveAndUI();
            GameManager.instance.dialogueManager.StartDialogue(dialogue, DoneTalkWithCharacter);

            void DoneTalkWithCharacter()
            {
                EnablePlayerMoveAndUI();
                player.ShowInteractButton();
            }

            onDone?.Invoke();
        }
    }

    public void PlayParticalEffect(int index, Vector3 pos)
    {
        effs[index].transform.position = pos;
        effs[index].Play();
    }

    public void DisablePlayerMoveAndUI()
    {
        player.DisableMove();
        player.HideUI();
    }

    public void EnablePlayerMoveAndUI()
    {
        player.EnableMove();
        player.ShowUI();
    }
}
