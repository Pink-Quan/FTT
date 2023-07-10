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

    private void Start()
    {
        switch (PlayerPrefs.GetString("Progress"))
        {
            case "Start Camping":
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
        Ngan.transform.position = playerStartCampingPos + Vector3.up * 2 + Vector3.left / 2;
        Minh.transform.position = playerStartCampingPos + Vector3.up * 2 + Vector3.right / 2;
        Mai.transform.position = playerStartCampingPos + Vector3.left;
        Hung.transform.position = playerStartCampingPos + Vector3.left * 2;
        Nam.transform.position = playerStartCampingPos + Vector3.right;

        player.anim.RandomDelayAnim();
        Ngan.anim.RandomDelayAnim();
        Minh.anim.RandomDelayAnim();
        Mai.anim.RandomDelayAnim();
        Hung.anim.RandomDelayAnim();
        Nam.anim.RandomDelayAnim();

        player.anim.SetDirection(Vector3.up);
        Ngan.anim.SetDirection(Vector3.down);
        Minh.anim.SetDirection(Vector3.down);
        Mai.anim.SetDirection(Vector3.up);
        Hung.anim.SetDirection(Vector3.up);
        Nam.anim.SetDirection(Vector3.up);
    }

    private void FirstComunicateWithManager()
    {
        Debug.Log("Comunicate with managers");
    }
}
