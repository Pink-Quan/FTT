using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        string sceneName = "";
        switch((GameProgress)PlayerPrefs.GetInt("Progress"))
        {
            case GameProgress.GoingHome:
                sceneName = "GoingHome";
                break;;
            case GameProgress.InsideLinhHouse:
                sceneName = "GoingHome";
                break;
            case GameProgress.StartCamping:
                sceneName = "Hospital";
                break;
            default:
                sceneName = "Hospital";
                //sceneName = "MainMap";
                break;
        }
        GameManager.instance.transitions.Transition(1, 1, null, () =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }

    public void Exit()
    {
        Application.Quit();
    }
}

public enum GameProgress
{
    InsideHospital=0,
    GoingHome=1,
    StartCamping=2,
    InsideLinhHouse=3,
}
