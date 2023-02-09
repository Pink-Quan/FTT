using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class DialogTests : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Dialogue dialogue2;

    private DialogManager dialogManager;

    private void Awake()
    {
        dialogManager = FindObjectOfType<DialogManager>();
    }
    private void Start()
    {
        dialogManager.StartDialogue(dialogue, End);
    }
    private void End()
    {
        dialogManager.StartQuesions(dialogue2, GetAnswer);
    }
    private void GetAnswer(List<int> ans)
    {
        for (int i = 0; i < ans.Count; i++)
            Debug.Log(ans[i]);
    }
}
