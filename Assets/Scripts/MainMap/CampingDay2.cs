using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampingDay2 : MonoBehaviour
{
    [SerializeField] Vector3 HungStartPos;
    [SerializeField] Vector3 MaiStartPos;
    [SerializeField] Vector3 NganStartPos;
    [SerializeField] Vector3 MinhStartPos;
    [SerializeField] Vector3 NamStartPos;
    [SerializeField] Vector3 playerStartPos;

    private CharacterController Minh;
    private CharacterController Nam;
    private CharacterController Hung;
    private CharacterController Ngan;
    private CharacterController Mai;
    private PlayerController player;

    private MainMapManager mainMapManager;
    private MainMapDay2Texts texts;
    public void Init(MainMapManager mainMapManager)
    {
        texts = Resources.Load<MainMapDay2Texts>($"Texts/MainMap/Day2/{PlayerPrefs.GetString("Language", "Eng")}");

        this.mainMapManager = mainMapManager;

        Minh = mainMapManager.Minh;
        Nam = mainMapManager.Nam;
        Hung = mainMapManager.Hung;
        Ngan = mainMapManager.Ngan;
        Mai = mainMapManager.Mai;
        player = mainMapManager.player;

        SetStartDay2Character();
    }

    private void SetStartDay2Character()
    {
        SetCharacterPos(Minh, MinhStartPos, Vector2.up);
        SetCharacterPos(Ngan, NganStartPos, Vector2.right);
        SetCharacterPos(Hung, HungStartPos, Vector2.up);
        SetCharacterPos(Nam, NamStartPos, Vector2.down);
        SetCharacterPos(player, playerStartPos, Vector2.down);
        //SetCharacterPos(Mai, MaiStartPos, Vector2.left);
        Mai.gameObject.SetActive(false);

        Minh.AddConversationToCharacter(texts.MinhFirstConversation);
        Nam.AddConversationToCharacter(texts.NamFirstConversation);
        Hung.AddConversationToCharacter(texts.HungFirstConversation);
        Minh.AddConversationToCharacter(texts.MinhFirstConversation);
    }

    private void SetCharacterPos(CharacterController character, Vector3 pos, Vector2 dir)
    {
        character.transform.position = pos;
        character.anim.SetDirection(dir);
    }
}
