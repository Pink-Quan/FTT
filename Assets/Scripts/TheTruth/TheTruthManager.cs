using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TheTruthManager : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    [SerializeField] GameObject continueButton;
    [SerializeField] CanvasGroup newsCanvasGroup;
    [SerializeField] TMP_Text newsText;
    [SerializeField] CharacterController Hung;
    [SerializeField] CharacterController HungBlack;
    [SerializeField] VectorPaths HungGetShowerPath;
    [SerializeField] GameObject HungRoom;
    [SerializeField] GameObject villaFirstFloor;

    [SerializeField] TMP_Text fewMonthsAgoText;
    [SerializeField] CanvasGroup fewMonthsAgoCanvasGroup;

    TheTruthText texts;
    void Start()
    {
        HungBlack.transform.position = Hung.transform.position;
        HungBlack.anim.SetDirection(Vector2.down);
        HungBlack.gameObject.SetActive(false);
        texts = Resources.Load<TheTruthText>($"Texts/TheTruth/{PlayerPrefs.GetString("Language", "Eng")}");
        newsText.text = texts.news;

        DOVirtual.DelayedCall(2,()=>continueButton.SetActive(true));
    }

    public void HungWatchNews()
    {
        mainCam.transform.DOMove(new Vector3(0, 0, -10), 2);
        mainCam.DOOrthoSize(3.4f, 2);
        newsCanvasGroup.DOFade(0, 2);
        DOVirtual.DelayedCall(2, () =>
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.HungFirstDialogue, HungGetShower);
        });
    }

    private void HungGetShower()
    {
        Hung.Move(HungGetShowerPath.paths, 4, () =>
        {
            Hung.gameObject.SetActive(false);
            GameManager.instance.transitions.Transition(1, 1, HungTellTheTruth, InitAfterHungGettingShower);
        });
    }

    private void InitAfterHungGettingShower()
    {
        HungBlack.gameObject.SetActive(true);
    }

    private void HungTellTheTruth()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.HungSecondDialogue, InitTheTruth);
    }

    private void InitTheTruth()
    {
        fewMonthsAgoText.text = texts.fewMonthsAgo;
        fewMonthsAgoCanvasGroup.gameObject.SetActive(true);
        fewMonthsAgoCanvasGroup.alpha = 0;
        fewMonthsAgoCanvasGroup.DOFade(1, 1).OnComplete(() =>
        {
            HungRoom.gameObject.SetActive(false);
            villaFirstFloor.gameObject.SetActive(true);
            HungBlack.gameObject.SetActive(false);
            canContinueTheTruth = true;
        });
    }

    bool canContinueTheTruth;

    public void ToTheTruth()
    {
        if (!canContinueTheTruth) return;
        canContinueTheTruth = false;
        fewMonthsAgoCanvasGroup.DOFade(0, 1).OnComplete(() =>
        {
            fewMonthsAgoText.gameObject.SetActive(false);
        });
    }
}
