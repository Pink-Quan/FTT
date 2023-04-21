using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Transitions : MonoBehaviour
{
    [SerializeField] private GameObject transitionParent;
    [SerializeField] private Image transitionImage;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private Button clickToContine;

    public void Transition(Action OnComplete)
    {
        Transition(1, 1, OnComplete);
    }

    public void Transition(float transTime, float stayTime, Action OnComplete)
    {
        transitionParent.SetActive(true);
        StartCoroutine(StartTrasition(transTime, stayTime, OnComplete));
    }

    IEnumerator StartTrasition(float transTime, float stayTime, Action OnComplete)
    {
        transitionImage.gameObject.SetActive(true);
        Color imgColor = transitionImage.color;
        imgColor.a = 0;
        transitionImage.color = imgColor;

        float clk = 0f;
        while (clk < transTime)
        {
            Color tImgColor = transitionImage.color;
            tImgColor.a = clk / transTime;
            transitionImage.color = tImgColor;

            clk += Time.deltaTime;
            yield return null;
        }

        clk = transTime;
        imgColor = transitionImage.color;
        imgColor.a = 1;
        transitionImage.color = imgColor;
        yield return new WaitForSeconds(stayTime);

        while (clk > 0)
        {
            Color tImgColor = transitionImage.color;
            tImgColor.a = clk / transTime;
            transitionImage.color = tImgColor;

            clk -= Time.deltaTime;
            yield return null;
        }

        imgColor = transitionImage.color;
        imgColor.a = 0;
        transitionImage.color = imgColor;

        OnComplete?.Invoke();
        transitionImage.gameObject.SetActive(false);
    }
    private Action OnDone;
    public void TransitionWithText(string text, UnityAction OnClick,Action OnDone)
    { 
        this.OnDone=OnDone;
        textUI.text = text;
        StartCoroutine(StartTrasitionInWithText(1));
        clickToContine.gameObject.SetActive(true);
        clickToContine.onClick.RemoveAllListeners();
        clickToContine.onClick.AddListener(OnClick);
    }

    IEnumerator StartTrasitionInWithText(float transTime)
    {
        transitionImage.gameObject.SetActive(true);
        Color color = transitionImage.color;
        color.a = 0;
        transitionImage.color = color;

        textUI.gameObject.SetActive(true);
        color = textUI.color;
        color.a = 0;
        textUI.color = color;

        float clk = 0f;
        while (clk < transTime)
        {
            Color tColor = transitionImage.color;
            tColor.a = clk / transTime;
            transitionImage.color = tColor;

            tColor = textUI.color;
            tColor.a = clk / transTime;
            textUI.color = tColor;

            clk += Time.deltaTime;
            yield return null;
        }

        color = transitionImage.color;
        color.a = 1;
        transitionImage.color = color;

        color = textUI.color;
        color.a = 1;
        textUI.color = color;
    }

    public void TransitionOutWithText()
    {
        StartCoroutine(StartTrasitionOutWithText(1));
    }
    IEnumerator StartTrasitionOutWithText(float transTime)
    {
        Color color = transitionImage.color;
        color.a = 1;
        transitionImage.color = color;

        color = textUI.color;
        color.a = 1;
        textUI.color = color;

        float clk = 0f;
        while (clk < transTime)
        {
            Color tColor = transitionImage.color;
            tColor.a = 1-clk / transTime;
            transitionImage.color = tColor;

            tColor = textUI.color;
            tColor.a = 1-clk / transTime;
            textUI.color = tColor;

            clk += Time.deltaTime;
            yield return null;
        }

        color = transitionImage.color;
        color.a = 0;
        transitionImage.color = color;
        transitionImage.gameObject.SetActive(false);

        color = textUI.color;
        color.a = 0;
        textUI.color = color;
        textUI.gameObject.SetActive(false);

        OnDone?.Invoke();
    }
}