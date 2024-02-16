using ExtendedAnimation;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private UIAnimationButton pauseButton;
    [SerializeField] private GameObject bg;
    [SerializeField] private GameObject pauseBoard;
    [SerializeField] private AudioMixer audioMixer;

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = Mathf.Clamp01(timeScale);
    }

    public void BackToMenu()
    {
        SetTimeScale(1);
        GameManager.instance.transitions.Transition(1, 1, null, () => 
        {
            pauseButton.Show();
            pauseBoard.SetActive(false);
            bg.SetActive(false);
            GameManager.instance.dialogueManager.CloseDialogueBoard();
            SceneManager.LoadScene("MainMenu");
        });
    }

    public void AdjustMusic(float value)
    {
        audioMixer.SetFloat("Music Volume", Mathf.Log10(value) * 20);

    }

    public void AdjustSFX(float value)
    {
        audioMixer.SetFloat("SFX Volume", Mathf.Log10(value) * 20);
    }

    public void SetFps(int fps)
    {
        Application.targetFrameRate = fps;
    }
}
