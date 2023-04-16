using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class DialogManager : MonoBehaviour
{
    private Queue<string> senctences = new Queue<string>();

    private void Awake()
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
        SellectNextButton.onClick.AddListener(DisplayNextSentance);
        Sellect1Button.gameObject.SetActive(false);
        Sellect2Button.gameObject.SetActive(false);
        Sellect3Button.gameObject.SetActive(false);
        Sellect4Button.gameObject.SetActive(false);
        SellectNextButton.gameObject.SetActive(false);
        DialogueBroad.SetActive(false);
    }

    [SerializeField] private Button SellectNextButton;
    [SerializeField] private Button Sellect1Button;
    [SerializeField] private Button Sellect2Button;
    [SerializeField] private Button Sellect3Button;
    [SerializeField] private Button Sellect4Button;
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private Image Avatar;
    [SerializeField] private GameObject DialogueBroad;

    [SerializeField] private GameObject baseAvatar;

    [Header("Animation")]
    [SerializeField] private RectTransform AppearPos;
    [SerializeField] private RectTransform DisappearPos;


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
            if (TriggerAnswerQuestions != null)
            {
                TriggerAnswerQuestions?.Invoke(AnswersList);
                TriggerAnswerQuestions = null;
            }
            CloseDialogue();
        }
    }

    public void StartDialogue(Dialogue dialogue, Action OnDoneDialogue)
    {
        this.OnDoneDialogue = OnDoneDialogue;

        SellectNextButton.gameObject.SetActive(true);
        SellectNextButton.onClick.RemoveAllListeners();
        SellectNextButton.onClick.AddListener(DisplayNextSentance);

        Sellect1Button.gameObject.SetActive(false);
        Sellect2Button.gameObject.SetActive(false);
        Sellect3Button.gameObject.SetActive(false);
        Sellect4Button.gameObject.SetActive(false);

        InitDialog(dialogue);

        DisplayNextSentance();
    }
    private void DisplayNextSentance()
    {
        if (senctences.Count > 0)
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
        displayingSectance = sentance;
        Text.text = "";
        int i = 0;
        while (i < sentance.Length)
        {
            isDialogueing = true;
            Text.text += sentance[i++];
            yield return new WaitForEndOfFrame();
        }
        isDialogueing = false;
    }

    private void InitDialog(Dialogue dialogue)
    {
        CancelInvoke();

        DialogueBroad.transform.DOMove(AppearPos.position, 0.5f);

        DialogueBroad.gameObject.SetActive(true);
        Name.text = dialogue.DisplayName;

        baseAvatar.SetActive(false);
        Destroy(charactorAvatar);
        var pb=Resources.Load<GameObject>(dialogue.GetResourseAddress());
        if (pb == null) baseAvatar.SetActive(true);
        else
        {
            charactorAvatar = Instantiate(pb);
            charactorAvatar.transform.SetParent(DialogueBroad.transform);
            charactorAvatar.transform.SetAsFirstSibling();
            charactorAvatar.transform.localScale = Vector3.one;

            var recChar=charactorAvatar.GetComponent<RectTransform>();
            var recBase=baseAvatar.GetComponent<RectTransform>();

            recChar.anchorMin = recBase.anchorMin;
            recChar.anchorMax = recBase.anchorMax;
            recChar.anchoredPosition3D = recBase.anchoredPosition3D;
            recChar.sizeDelta = recBase.sizeDelta;

        }

        senctences?.Clear();
        foreach (string sentance in dialogue.sentances)
        {
            senctences.Enqueue(sentance);
        }
    }

    private void CloseDialogue()
    {
        if (canClose) DialogueBroad.transform.DOMove(DisappearPos.position, 0.5f);
        Invoke("OffDialogue", 0.5f);
        OnDoneDialogue?.Invoke();
    }

    private bool canClose = true;
    public void DontCloseDialogueBoard()
    {
        canClose = false;
    }

    public void CloseDialogueBoard()
    {
        canClose = true;
        DialogueBroad.transform.DOMove(DisappearPos.position, 0.5f);
    }

    private void OffDialogue()
    {
        DialogueBroad.SetActive(false);
        Sellect1Button.gameObject.SetActive(false);
        Sellect2Button.gameObject.SetActive(false);
        Sellect3Button.gameObject.SetActive(false);
        Sellect4Button.gameObject.SetActive(false);
        SellectNextButton.gameObject.SetActive(false);
    }

    Queue<MultiCharactorDialogue.CharacterPerDialogue> multiDialogueQueue;
    public void MultiCharactorDialogue(MultiCharactorDialogue dialogue, Action OnDoneDialogue)
    {
        SellectNextButton.gameObject.SetActive(true);
        SellectNextButton.onClick.RemoveAllListeners();
        SellectNextButton.onClick.AddListener(DisplayNextSentance);

        Sellect1Button.gameObject.SetActive(false);
        Sellect2Button.gameObject.SetActive(false);
        Sellect3Button.gameObject.SetActive(false);
        Sellect4Button.gameObject.SetActive(false);

        multiDialogueQueue = new Queue<MultiCharactorDialogue.CharacterPerDialogue>(dialogue.dialogues);
    }

    Queue<Dialogue> squenceDialogue;
    Action OnDoneAllDialogues;
    public void StartSequanceDialogue(Dialogue[] squenceDialogue, Action OnDoneAllDialogues)
    {
        this.squenceDialogue = new Queue<Dialogue>(squenceDialogue);
        this.OnDoneAllDialogues = OnDoneAllDialogues;
        DontCloseDialogueBoard();
        SequanceDialogue();
    }

    private void SequanceDialogue()
    {
        if(squenceDialogue.Count==1)
            StartDialogue(squenceDialogue.Dequeue(), ()=>
            {
                CloseDialogueBoard();
                OnDoneAllDialogues?.Invoke();
            });
        else
        StartDialogue(squenceDialogue.Dequeue(), SequanceDialogue);
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
    [Serializable]
    public enum Emotion
    {
        NoEmo = 0,
        Happy = 1,
        Fear = 2
    }

    public string GetResourseAddress()
    {
        return $"{Name}/{Name}_{emotion}";
    }
}

[Serializable]
public struct MultiCharactorDialogue
{
    [Serializable]
    public struct CharacterPerDialogue
    {
        public string Text;
        public string Name;
        public GameObject Avatar;
    }

    public CharacterPerDialogue[] dialogues;

}

