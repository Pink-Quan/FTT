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
    private void Start()
    {
        DialogManger.instance.StartDialogue(dialogue,End);
    }
    private void End()
    {
        DialogManger.instance.StartQuesions(dialogue2,GetAnswer);
    }
    private void GetAnswer(List<int> ans)
    {
        for(int i = 0; i < ans.Count; i++)
            Debug.Log(ans[i]);
    }
}
