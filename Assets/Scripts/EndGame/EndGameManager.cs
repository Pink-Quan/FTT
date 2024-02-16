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

        DOVirtual.DelayedCall(delayToMenu, () =>
        {
            GameManager.instance.transitions.Transition(1, 1,
                () => GameManager.instance.pauseButton.gameObject.SetActive(true),
                () => SceneManager.LoadScene("MainMenu"));
        });
    }

}
