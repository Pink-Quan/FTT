using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text inforText;
    private MainMenuTexts texts;
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

    private void Start()
    {
        
    }

    public void Exit()
    {
        GameManager.instance.transitions.Transition(1, 1, null, () => { Application.Quit(); });
    }

    public void ResetProgress()
    {
        PlayerPrefs.SetInt("Progress", 0);
    }

    public void UpdateInfoText()
    {
        inforText.text = texts.information;
    }

    public void ChanngeLanguage(string language)
    {
        PlayerPrefs.SetString("Language", language);

    }
}

public enum GameProgress
{
    InsideHospital=0,
    GoingHome=1,
    StartCamping=2,
    InsideLinhHouse=3,
}
