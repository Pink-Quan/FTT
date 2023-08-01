using SuperTiled2Unity.Editor.ClipperLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingMenu : MonoBehaviour
{
    public static TestingMenu instance;

    public Canvas canvas;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void SetFPS(int fps)
    {
        Application.targetFrameRate = fps;
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void BackToMainScene()
    {
        SceneManager.LoadScene(0);
    }

    public void ClearPlayerPrefsData()
    {
        PlayerPrefs.DeleteAll();
    }
}
