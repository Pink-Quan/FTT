using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackLinhsHomeManager : MonoBehaviour
{
    [SerializeField] private LoopStreet loopStreet;
    private ComeBackLinhsHomeText texts;
    private void Start()
    {
        texts = Resources.Load<ComeBackLinhsHomeText>("Texts/BackToLinhsHome/Viet");
        Invoke("StartConversations", 2);
    }

    private void StartConversations()
    {
        GameManager.instance.dialogManager.StartDialogue(texts.conversationBetweenHelperAndLinh, ()=>Invoke("AboutToHomeConversations",1));
    }

    private void AboutToHomeConversations()
    {
        loopStreet.ChangeSpeed(1f / 2);
        GameManager.instance.dialogManager.StartDialogue(squenceDialogue:texts.aboutToGoHome,OnDoneAllDialogues:TransitionsToLinhHouse);
    }

    private void TransitionsToLinhHouse()
    {
        GameManager.instance.transitions.Transition(1,1,null,ChangeToLinhHouseScene);
    }

    private void ChangeToLinhHouseScene()
    {
        PlayerPrefs.SetInt("Progress", (int)GameProgress.InsideLinhHouse);
        SceneManager.LoadScene("LinhHouse");
    }
}
