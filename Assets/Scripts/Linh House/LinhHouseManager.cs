using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinhHouseManager : MonoBehaviour
{
    private PlayerController player;
    private LinhHouseTexts texts;
    private void Start()
    {
        player = GameManager.instance.player;
        player.DisableMove();
        player.HideButtons();
        player.stress.HideStressBar();
        player.stress.HideBreathButton();
        Invoke("FirstSeftConversation", 2);

        texts = Resources.Load<LinhHouseTexts>("Texts/Linh House/Viet");
    }

    private void FirstSeftConversation()
    {
        GameManager.instance.dialogManager.StartDialogue(texts.firstSeftDialogue, PlayerExploreHouse);
    }

    private void PlayerExploreHouse()
    {
        player.ShowButtons();
        player.EnableMove();
    }
}
