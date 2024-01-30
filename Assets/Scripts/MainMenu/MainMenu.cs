using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loading;

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
                sceneName = "LinhHouse";
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

    private void PlayBGMucis()
    {
        GameManager.instance.soundManager.PlayMusic("BG");
    }

    private void Start()
    {
        texts = Resources.Load<MainMenuTexts>("Texts/MainMenu/" + PlayerPrefs.GetString("Language", "Eng"));
        PlayBGMucis();
    }

    public void Exit()
    {
        loading.SetActive(true);
        GameManager.instance.dbManager.UpdateDB(Application.Quit);
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

    public void GetEmail()
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = "contact.lily.84@gmail.com";
        textEditor.SelectAll();
        textEditor.Copy();
        GameManager.instance.textBoard.ShowText("Email copied to clipboard <br>contact.lily.84@gmail.com");
    }

    public void PlayClickSound()
    {
        GameManager.instance.soundManager.PlayCommondSound("Click");
    }

    public void GetAnnouncement()
    {
        loading.SetActive(true);
        GameManager.instance.dbManager.GetAnnouncement((bool isSuccess, string result) =>
        {
            if(!loading.activeSelf) return;

            loading.SetActive(false);
            if (!isSuccess)
            {
                GameManager.instance.textBoard.ShowText("Fail to load Announment");
            }
            else
            {
                GameManager.instance.textBoard.ShowText(result);
            }

        });
    }

    public void ShowGuide()
    {
        GameManager.instance.textBoard.ShowText(texts.howToPlay);
    }
}

public enum GameProgress
{
    InsideHospital=0,
    GoingHome=1,
    InsideLinhHouse=3,
    StartCamping=2,
    CampingDay2=4,
    CampingDay3=5,
    Chess = 6,
}
