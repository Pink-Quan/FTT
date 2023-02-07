using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class DialogManger : MonoBehaviour
{
    public static DialogManger instance;
    private Queue<string> senctences;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }    
        else
        {
            Destroy(gameObject);
        }    
        //DontDestroyOnLoad(gameObject);
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

    [Header("Animation")]
    [SerializeField] private RectTransform AppearPos;
    [SerializeField] private RectTransform DisappearPos;


    private Action TriggerEndConversations;
    private Action<int> TriggerAnswerQuestion;
    private Action<List<int>> TriggerAnswerQuestions;
    private List<int> AnswersList;
    public void Start1Quesion(Dialogue dialogue,Action<int> TriggerAnswerQuestion)
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
        if(TriggerAnswerQuestion != null)
        {
            TriggerAnswerQuestion(But);
            TriggerAnswerQuestion=null;
            CloseDialogue();
        }    
        else if(TriggerAnswerQuestions!=null)
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
        if (senctences.Count > 0)
        {
            StartCoroutine(DisplaySentance(senctences.Dequeue()));
        }
        else
        {
            if (TriggerAnswerQuestions!=null)
            {
                TriggerAnswerQuestions?.Invoke(AnswersList);
                TriggerAnswerQuestions = null;
            }    
            CloseDialogue();
        }
    }

    public void StartDialogue(Dialogue dialogue, Action TriggerEndConversations)
    {
        this.TriggerEndConversations = TriggerEndConversations;

        SellectNextButton.gameObject.SetActive(true);
        
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
    IEnumerator DisplaySentance(string sentance)
    {
        Text.text = "";
        int i = 0;
        while(i< sentance.Length)
        {
            yield return new WaitForEndOfFrame();
            Text.text += sentance[i++];
        }
    }

    private void InitDialog(Dialogue dialogue)
    {
        CancelInvoke();

        DialogueBroad.transform.DOMove(AppearPos.position, 0.5f);

        DialogueBroad.gameObject.SetActive(true);
        Avatar.sprite = dialogue.Avatar;
        Name.text = dialogue.Name;

        senctences.Clear();
        foreach (string sentance in dialogue.sentances)
        {
            senctences.Enqueue(sentance);
        }
    }

    private void CloseDialogue()
    { 
        DialogueBroad.transform.DOMove(DisappearPos.position, 0.5f);
        Invoke("OffDialogue", 0.5f);
        if (TriggerEndConversations!=null)
        {
            TriggerEndConversations?.Invoke();
            TriggerEndConversations=null;
        }    
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

}
[System.Serializable]
public class Dialogue
{
    [TextArea(1, 10)]
    public string[] sentances;
    public string Name;
    public Sprite Avatar;
}

