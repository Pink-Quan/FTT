using Cinemachine;
using QuanUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        
        input = new PlayerInput();
        input.Enable();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitGameManager()
    {
        Application.targetFrameRate = 60;
        Instantiate(Resources.Load<GameObject>("GameManager"));
    }

    public DialogueManager dialogueManager;
    public Transitions transitions;
    public PlayerController player;
    public TextBoard textBoard;
    public PlayerInput input;
    public FastNotification fastNotification;
    public DBManager dbManager;
    public PauseMenu pauseMenu;
    public SoundManager soundManager;
    public Missions missionsManager;
    public ShowImage showImage;
    public GameObject pauseButton;

    private void OnDestroy()
    {
        instance = null;
    }

    public void DisablePlayerMoveAndUI()
    {
        player.HideUI();
        player.DisableMove();
    }

    public void EnablePlayerMoveAndUI()
    {
        player.ShowUI();
        player.EnableMove();
    }

    public void CineCamShake(CinemachineVirtualCamera cam,float amplitude,float duration)
    {
        var shake = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        shake.m_AmplitudeGain = amplitude;
        this.DelayFunction(duration, () =>
        {
            shake.m_AmplitudeGain = 0;
        });
    }

    public void ResetGameData()
    {
        PlayerPrefs.SetInt("Progress", 0);
        PlayerPrefs.SetInt("ProgressDay3", 0);
        PlayerPrefs.SetInt("Restart Hosptal After Camping", 0);
    }
}
