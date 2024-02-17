using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TheTruthManager : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    [SerializeField] GameObject continueButton;
    [SerializeField] CanvasGroup newsCanvasGroup;
    [SerializeField] TMP_Text newsText;
    [SerializeField] CharacterController Hung;
    [SerializeField] CharacterController HungBlack;
    [SerializeField] CharacterController Linh;
    [SerializeField] CharacterController killer;
    [SerializeField] VectorPaths HungGetShowerPath;
    [SerializeField] GameObject HungRoom;
    [SerializeField] GameObject villaFirstFloor;

    [SerializeField] TMP_Text fewMonthsAgoText;
    [SerializeField] CanvasGroup fewMonthsAgoCanvasGroup;
    [SerializeField] Vector3 initHungThePastPos = new Vector3(-4.25523f, -0.8667538f);
    [SerializeField] VectorPaths pathHungMoveToKitchen;
    [SerializeField] VectorPaths pathHungMoveBackToLivingRoom;
    [SerializeField] VectorPaths killerPath;
    [SerializeField] GameObject HungBlood;
    [SerializeField] Image black;
    [SerializeField] Vector3 hungInjuryPos = new Vector3(-2.84f, 5.61f);

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
        newsCanvasGroup.DOFade(0, 2).OnComplete(()=>newsCanvasGroup.gameObject.SetActive(false));
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
            Hung.gameObject.SetActive(true);
            HungRoom.gameObject.SetActive(false);
            villaFirstFloor.gameObject.SetActive(true);
            HungBlack.gameObject.SetActive(false);
            canContinueTheTruth = true;
            Hung.SetPositon(initHungThePastPos, Vector2.right);
            mainCam.transform.SetParent(Hung.transform, true);
            mainCam.transform.localPosition = new Vector3(0, 0, -10);
            Linh.gameObject.SetActive(true);
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
            HungResolveTheQuest();

        });
    }

    private void HungResolveTheQuest()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.HungResolveTheQuestWithLinh, HungMoveToKitchenAndTellTheTruth);
    }

    private void HungMoveToKitchenAndTellTheTruth()
    {
        Hung.Move(pathHungMoveToKitchen.paths, 5, () =>
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.HungRememberThePast, HungBackToLivingRoom);
            Linh.gameObject.SetActive(false);
        });
    }

    private void HungBackToLivingRoom()
    {
        Hung.Move(pathHungMoveBackToLivingRoom.paths, 5, () =>
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.HungAskWhereIsLinh, KillerFightWithHung);
        });
    }

    private void KillerFightWithHung()
    {
        killer.gameObject.SetActive(true);
        killer.Move(killerPath.paths, 15, HungFightWithKiller);
    }

    private void HungFightWithKiller()
    {
        black.gameObject.SetActive(true);
        Hung.anim.StopAnimation();
        Hung.transform.position = hungInjuryPos;
        Hung.transform.rotation = Quaternion.Euler(0, 0, 90);
        HungBlood.SetActive(true);
        mainCam.transform.rotation = Quaternion.identity;
        black.DOFade(0, 3).OnComplete(() =>
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.killerDialogue, ChangeToEndGameScene);
        });
    }

    private void ChangeToEndGameScene()
    {
        PlayerPrefs.SetInt("Found the truth", 1);
        PlayerPrefs.SetInt("Progress", (int)GameProgress.EndGame);
        GameManager.instance.dbManager.UpdateDB();
        GameManager.instance.transitions.Transition(1, 1, null, () => SceneManager.LoadScene("EndGame"));
    }
}
