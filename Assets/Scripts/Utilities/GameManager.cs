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

    public DialogManager dialogManager;
    public Transitions transitions;
    public PlayerController player;
    public TextBoard textBoard;
    public PlayerInput input;
    public FastNotification fastNotification;

    private void OnDestroy()
    {
        instance = null;
    }

}
