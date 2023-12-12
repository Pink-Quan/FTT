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

    public void Init()
    {
        mainMapManager = GetComponent<MainMapManager>();
        player = GameManager.instance.player;
        player.DisableMoveAndUI();
        GameManager.instance.transitions.Transition(1, 1, LinhMonodiaglogueFirstMission, MovePlayerToFirstMission);
    }

    private void MovePlayerToFirstMission()
    {
        player.SetPositon(playerFirstMissionPos, Vector2.down);
    }

    private void LinhMonodiaglogueFirstMission()
    {
        Debug.Log("First mission day 3");
    }
}
