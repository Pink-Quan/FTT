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

}
