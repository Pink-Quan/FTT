using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] TMP_Text thatsTheTruth;
    [SerializeField] TMP_Text butOnlyOneKnow;
    [SerializeField] TMP_Text thanks;
    [SerializeField] TMP_Text comment;
    [SerializeField] TMP_Text takeCake;

    [SerializeField] float delayToMenu = 20f;

    EndGameTexts texts;
    void Start()
    {
        GameManager.instance.pauseButton.gameObject.SetActive(false);
        texts = Resources.Load<EndGameTexts>($"Texts/EndGame/{PlayerPrefs.GetString("Language", "Eng")}");

        thatsTheTruth.text = texts.thatsTheTruth;
        butOnlyOneKnow.text = texts.butOnlyOneKnow;
        thanks.text = texts.thanks;
        comment.text = texts.comment;
        takeCake.text = texts.takeCare;

        if (delayToMenu > 0)
            DOVirtual.DelayedCall(delayToMenu, ExitToMenu);
    }

    public void ExitToMenu()
    {
        GameManager.instance.transitions.Transition(1, 1,
                () => GameManager.instance.pauseButton.gameObject.SetActive(true),
                () => SceneManager.LoadScene("MainMenu"));
    }

}
