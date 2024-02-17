using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loading;
    [SerializeField] private Button inforButton;

    [HideInInspector] public MainMenuTexts texts;

    public void StartGame()
    {
        GameManager.instance.soundManager.StopMusic();
        string sceneName = "";
        switch ((GameProgress)PlayerPrefs.GetInt("Progress", 0))
        {
            case GameProgress.GoingHome:
                sceneName = "GoingHome";
                break; ;
            case GameProgress.InsideLinhHouse:
                sceneName = "LinhHouse";
                break;
            case GameProgress.StartCamping:
            case GameProgress.CampingDay2:
            case GameProgress.CampingDay3:
            case GameProgress.CampingDay4:
                sceneName = "MainMap";
                break;
            case GameProgress.Cave:
                sceneName = "Cave";
                break;
            case GameProgress.HosptalAfterCamping:
                sceneName = "HospitalAfterCamping";
                break;
            case GameProgress.FightWithNam:
                sceneName = "FightWithNam";
                break;
            case GameProgress.ChaseLinh:
                sceneName = "MainMapCatchLinh";
                break;
            case GameProgress.TheTruth:
                sceneName = "TheTruth";
                break;
            case GameProgress.EndGame:
                sceneName = "EndGame";
                break;
            default:
                sceneName = "Hospital";
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
        inforButton.onClick.Invoke();
    }

    public void Exit()
    {
        loading.SetActive(true);
        GameManager.instance.dbManager.UpdateDB(Application.Quit);
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

    public void OpenUri(string uri)
    {
        Application.OpenURL(uri);
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
            if (!loading.activeSelf) return;

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

    public void RestartGame()
    {
        GameManager.instance.ResetGameData();
    }
}

public enum GameProgress
{
    InsideHospital = 0,
    GoingHome = 1,
    InsideLinhHouse = 2,
    StartCamping = 3,
    CampingDay2 = 4,
    CampingDay3 = 5,
    CampingDay4 = 6,
    Cave = 7,
    HosptalAfterCamping = 8,
    FightWithNam = 9,
    ChaseLinh = 10,
    TheTruth = 11,
    EndGame = 12,
}
