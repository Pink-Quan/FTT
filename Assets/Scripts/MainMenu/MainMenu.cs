using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [HideInInspector] public MainMenuTexts texts;
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
        texts = Resources.Load<MainMenuTexts>("Texts/MainMenu/" + PlayerPrefs.GetString("Language", "Eng"));
    }

    public void Exit()
    {
        GameManager.instance.transitions.Transition(1, 1, null, () => { Application.Quit(); });
    }

    public void ResetProgress()
    {
        PlayerPrefs.SetInt("Progress", 0);
    }

    public void ChanngeLanguage(string language)
    {
        PlayerPrefs.SetString("Language", language);
        texts = Resources.Load<MainMenuTexts>("Texts/MainMenu/" + PlayerPrefs.GetString("Language", "Eng"));
    }

    public void OpenFacebook()
    {
        Application.OpenURL("https://www.facebook.com/profile.php?id=100089991012819");
    }

    public void CopyEmail()
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = "contact.lily.84@gmail.com";
        textEditor.SelectAll();
        textEditor.Copy();
    }
}

public enum GameProgress
{
    InsideHospital=0,
    GoingHome=1,
    StartCamping=2,
    InsideLinhHouse=3,
}
