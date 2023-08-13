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

    [SerializeField] private StartCamping startCamping;

    private MainMapTexts texts;

    private void Start()
    {
        texts = Resources.Load<MainMapTexts>($"Texts/MainMap/{PlayerPrefs.GetString("Language", "Eng")}");
        switch ((GameProgress)PlayerPrefs.GetInt("Progress"))
        {
            case GameProgress.StartCamping:
                startCamping.Init(Ngan,Minh,Mai,Nam,Hung,player,texts);
                break;
        }
    }
}
