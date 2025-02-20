using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> senctences = new Queue<string>();

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        senctences = new Queue<string>();
        Sellect1Button.onClick.AddListener(() => SellectAnswer(1));
        Sellect2Button.onClick.AddListener(() => SellectAnswer(2));
        Sellect3Button.onClick.AddListener(() => SellectAnswer(3));
        Sellect4Button.onClick.AddListener(() => SellectAnswer(4));
        SellectNoButton.onClick.AddListener(() => onAnswerYesNo?.Invoke(false));
        SellectYesButton.onClick.AddListener(() => onAnswerYesNo?.Invoke(true));
        SellectNextButton.onClick.AddListener(DisplayNextSentance);
        Sellect1Button.gameObject.SetActive(false);
        Sellect2Button.gameObject.SetActive(false);
        Sellect3Button.gameObject.SetActive(false);
        Sellect4Button.gameObject.SetActive(false);
        SellectNextButton.gameObject.SetActive(false);
        SellectYesButton.gameObject.SetActive(false);
        SellectNoButton.gameObject.SetActive(false);
        DialogueBroad.gameObject.SetActive(false);

        //submitInput = GameManager.instance.input.UI.Submit;
    }

    //private InputAction submitInput;

    [SerializeField] private Button SellectNextButton;
    [SerializeField] private Button Sellect1Button;
    [SerializeField] private Button Sellect2Button;
    [SerializeField] private Button Sellect3Button;
    [SerializeField] private Button Sellect4Button;
    [SerializeField] private Button SellectYesButton;
    [SerializeField] private Button SellectNoButton;
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private Image Avatar;
    [SerializeField] private RectTransform DialogueBroad;

    [SerializeField] private GameObject baseAvatar;

    private Dialogue currentDialogue;
    private Action OnDoneDialogue;
    private Action<int> TriggerAnswerQuestion;
    private Action<List<int>> TriggerAnswerQuestions;
    private List<int> AnswersList;

    private GameObject charactorAvatar;


    public void Start1Quesion(Dialogue dialogue, Action<int> TriggerAnswerQuestion)
    {
        InitDialog(dialogue);
        Sellect1Button.gameObject.SetActive(true);
        Sellect2Button.gameObject.SetActive(true);
        Sellect3Button.gameObject.SetActive(true);
        Sellect4Button.gameObject.SetActive(true);
        SellectNextButton.gameObject.SetActive(false);
        SellectYesButton.gameObject.SetActive(false);
        SellectNoButton.gameObject.SetActive(false);

        this.TriggerAnswerQuestion = TriggerAnswerQuestion;

        StartCoroutine(DisplaySentance(senctences.Dequeue()));
    }
    private void SellectAnswer(int But)
    {
        if (TriggerAnswerQuestion != null)
        {
            TriggerAnswerQuestion(But);
            TriggerAnswerQuestion = null;
            CloseDialogue();
        }
        else if (TriggerAnswerQuestions != null)
        {
            AnswersList.Add(But);
            DisplayNextQuesion();
        }
    }

    public void StartQuesions(Dialogue dialogue, Action<List<int>> TriggerAnswerQuestions)
    {
        InitDialog(dialogue);
        AnswersList = new List<int>();
        Sellect1Button.gameObject.SetActive(true);
        Sellect2Button.gameObject.SetActive(true);
        Sellect3Button.gameObject.SetActive(true);
        Sellect4Button.gameObject.SetActive(true);

        this.TriggerAnswerQuestions = TriggerAnswerQuestions;
        DisplayNextQuesion();
    }

    public void StartQuestion(Dialogue dialogue, Action<bool> onAnswerYesNo, string yesText = "Yes", string noText = "No")
    {
        SellectNoButton.gameObject.SetActive(true);
        SellectYesButton.gameObject.SetActive(true);
        SellectNextButton.gameObject.SetActive(false);
        InitDialog(dialogue);
        onAnswerYesNo += isYes => CloseDialogue();
        this.onAnswerYesNo = onAnswerYesNo;

    }
    private Action<bool> onAnswerYesNo;
    private void DisplayNextQuesion()
    {
        if (isDialogueing)
        {
            StopAllCoroutines();
            Text.text = displayingSectance;
        }
        else if (senctences.Count > 0)
        {
            StartCoroutine(DisplaySentance(senctences.Dequeue()));
        }
        else
        {
            TriggerAnswerQuestions?.Invoke(AnswersList);
            TriggerAnswerQuestions = null;
            CloseDialogue();
        }
    }

    public void StartDialogue(Dialogue dialogue, Action OnDoneDialogue)
    {
        //submitInput.performed += ctx => DisplayNextSentance();
        //OnDoneDialogue += () => { submitInput.performed -= ctx => DisplayNextSentance(); };
        if (dialogue.sentances.Length == 0)
            Debug.LogWarning("Dialogue have no sentance, OnDoneDialogue will call immediately");

        this.OnDoneDialogue = OnDoneDialogue;
        currentDialogue = dialogue;

        SellectNextButton.gameObject.SetActive(true);
        SellectNextButton.onClick.RemoveAllListeners();
        SellectNextButton.onClick.AddListener(DisplayNextSentance);

        Sellect1Button.gameObject.SetActive(false);
        Sellect2Button.gameObject.SetActive(false);
        Sellect3Button.gameObject.SetActive(false);
        Sellect4Button.gameObject.SetActive(false);
        SellectYesButton.gameObject.SetActive(false);
        SellectNoButton.gameObject.SetActive(false);

        InitDialog(dialogue);

        DisplayNextSentance();

        currentDialogue.onStart?.Invoke();
    }
    private void DisplayNextSentance()
    {
        if (isDialogueing)
        {
            StopAllCoroutines();
            Text.text = displayingSectance;
            isDialogueing = false;
        }
        else if (senctences.Count > 0)
        {
            StartCoroutine(DisplaySentance(senctences.Dequeue()));
        }
        else
        {
            CloseDialogue();
        }
    }
    private bool isDialogueing;
    private string displayingSectance;
    IEnumerator DisplaySentance(string sentance)
    {
        var sound = StartCoroutine(PlayDialogSound());
        displayingSectance = sentance;
        Text.text = "";
        int i = 0;
        while (i < sentance.Length)
        {
            isDialogueing = true;
            Text.text += sentance[i++];
            yield return new WaitForSeconds(0.02f);
        }
        StopCoroutine(sound);
        Text.text = displayingSectance;
        isDialogueing = false;
    }

    IEnumerator PlayDialogSound()
    {
        float time = GameManager.instance.soundManager.CommonSounds.sounds["Dialogue"].length / 3;
        while (true)
        {
            GameManager.instance.soundManager.PlayCommondSound("Dialogue");
            yield return new WaitForSeconds(time);
        }
    }

    private void InitDialog(Dialogue dialogue)
    {
        CancelInvoke();
        DOTween.Kill(this);
        DialogueBroad.DOAnchorPosY(DialogueBroad.sizeDelta.y / 2, 0.5f);

        DialogueBroad.gameObject.SetActive(true);
        Name.text = dialogue.DisplayName;

        baseAvatar.SetActive(false);
        Destroy(charactorAvatar);
        var pb = Resources.Load<GameObject>(dialogue.GetResourseAddress());
        if (pb == null) baseAvatar.SetActive(true);
        else
        {
            charactorAvatar = Instantiate(pb, DialogueBroad.transform, false);
            charactorAvatar.transform.SetAsFirstSibling();
            Vector3 lastScale = charactorAvatar.transform.localScale;
            Vector3 avaScale = Vector3.one;
            avaScale.x = Mathf.Abs(lastScale.x) / lastScale.x;
            avaScale.y = Mathf.Abs(lastScale.y) / lastScale.y;
            avaScale.z = Mathf.Abs(lastScale.z) / lastScale.z;
            charactorAvatar.transform.localScale = avaScale;

            var recChar = charactorAvatar.GetComponent<RectTransform>();
            var recBase = baseAvatar.GetComponent<RectTransform>();

            recChar.anchorMin = recBase.anchorMin;
            recChar.anchorMax = recBase.anchorMax;

            recChar.anchoredPosition = new Vector2(recChar.sizeDelta.x/2, recChar.sizeDelta.y/2);
        }

        senctences?.Clear();
        foreach (string sentance in dialogue.sentances)
        {
            senctences.Enqueue(sentance);
        }
    }

    private void CloseDialogue()
    {
        if (canClose) DialogueBroad.DOAnchorPosY(-DialogueBroad.sizeDelta.y * 5, 0.5f);
        Invoke("OffDialogue", 0.5f);
        OnDoneDialogue?.Invoke();
        currentDialogue.onDone?.Invoke();
        currentDialogue = new Dialogue();
    }

    private bool canClose = true;
    public void DontCloseDialogueBoard()
    {
        canClose = false;
    }

    public void CloseDialogueBoard()
    {
        canClose = true;
        DialogueBroad.DOAnchorPosY(-DialogueBroad.sizeDelta.y * 5, 0.5f);
    }

    private void OffDialogue()
    {
        DialogueBroad.gameObject.SetActive(false);
        Sellect1Button.gameObject.SetActive(false);
        Sellect2Button.gameObject.SetActive(false);
        Sellect3Button.gameObject.SetActive(false);
        Sellect4Button.gameObject.SetActive(false);
        SellectNextButton.gameObject.SetActive(false);
        SellectYesButton.gameObject.SetActive(false);
        SellectNoButton.gameObject.SetActive(false);
    }

    Queue<Dialogue> squenceDialogue;
    Action OnDoneAllDialogues;
    public void StartDialogue(Dialogue[] squenceDialogue, Action OnDoneAllDialogues)
    {
        this.squenceDialogue = new Queue<Dialogue>(squenceDialogue);
        this.OnDoneAllDialogues = OnDoneAllDialogues;
        DontCloseDialogueBoard();
        SequenceDialogue();
    }

    private void SequenceDialogue()
    {
        if (squenceDialogue.Count == 1)
            StartDialogue(squenceDialogue.Dequeue(), () =>
            {
                CloseDialogueBoard();
                OnDoneAllDialogues?.Invoke();
            });
        else
            StartDialogue(squenceDialogue.Dequeue(), SequenceDialogue);
    }
}
[Serializable]
public struct Dialogue
{
    [TextArea(1, 10)]
    public string[] sentances;
    public string Name;
    public string DisplayName;
    public Emotion emotion;

    public string keyValue;
    public Action onDone;
    public Action onStart;

    [Serializable]
    public enum Emotion
    {
        Angry = 3,
        Confident = 4,
        Fear = 2,
        Happy = 1,
        Normal = 0,
        Shocked = 5,
        Serious = 6,
        Sad = 7,
    }

    public string GetResourseAddress()
    {
        return $"Charactor Avatar/{Name}/{Name}_{emotion}";
    }
}

public static class DialogueHelper 
{ 
    public static int FindDiague(this Dialogue[] dialogue,string key)
    {
        for (int i = 0;i<dialogue.Length;i++)
            if (string.Compare(dialogue[i].keyValue, key) == 0)
                return i;
        return -1;
    }
}


