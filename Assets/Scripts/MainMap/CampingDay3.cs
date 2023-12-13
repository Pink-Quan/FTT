using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampingDay3 : MonoBehaviour
{
    [SerializeField] private GameObject mainHouse;
    [SerializeField] private GameObject mainMap;
    [SerializeField] private Vector3 playerFirstMissionPos;

    PlayerController player;
    MainMapManager mainMapManager;
    CampingDay3Text texts;

    public void Init()
    {
        mainMapManager = GetComponent<MainMapManager>();
        texts = Resources.Load<CampingDay3Text>($"Texts/MainMap/Day3/{PlayerPrefs.GetString("Language", "Eng")}");
        player = GameManager.instance.player;
        player.DisableMoveAndUI();
        GameManager.instance.transitions.Transition(1, 1, LinhMonodiaglogueFirstMission, MovePlayerToFirstMission);
    }

    private void MovePlayerToFirstMission()
    {
        mainMapManager.Mai.gameObject.SetActive(false);
        mainMapManager.Nam.gameObject.SetActive(false);
        mainMapManager.Minh.gameObject.SetActive(false);
        mainMapManager.Ngan.gameObject.SetActive(false);
        mainMapManager.Hung.gameObject.SetActive(false);
        mainMap.SetActive(false);
        mainHouse.SetActive(true);
        player.SetPositon(playerFirstMissionPos, Vector2.down);
    }

    private void LinhMonodiaglogueFirstMission()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.playerFirstDialgue, StartFirstMission);
    }

    private void StartFirstMission()
    {
        GameManager.instance.missionsManager.AddAndShowMission(texts.firstMissionText, () =>
        {
            player.EnableMoveAndUI();
        });
    }
}
