using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackLinhsHomeManager : MonoBehaviour
{
    private ComeBackLinhsHomeText texts;
    private void Start()
    {
        Invoke("StartConversations", 2);
    }

    private void StartConversations()
    {
        GameManager.instance.dialogManager.StartSequanceDialogue(texts.conversationBetweenHelperAndLinh, null);
    }
}
