using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HospitalAfterCampingManager : MonoBehaviour
{
    public PlayerController player;
    public CharacterController nurse;
    public CharacterController Mai;

    [SerializeField] VectorPaths nursePath;
    [SerializeField] InteractableEntity toEntrance;
    [SerializeField] GameObject MaiBlood;
    [SerializeField] TMP_Text clock;
    [SerializeField] int escapeDuration = 120;
    [SerializeField] GameObject failBoard;
    [SerializeField] TMP_Text failedText;

    private HospitalAfterCampingTexts texts;

    private IEnumerator Start()
    {
        texts = Resources.Load<HospitalAfterCampingTexts>($"Texts/HospitalAfterCamping/{PlayerPrefs.GetString("Language", "Eng")}");
        player.DisableMoveAndUI();
        player.anim.StopAnimation();
        Mai.anim.StopAnimation();
        toEntrance.canInteract = false;
        player.anim.SpriteRenderer.material.SetFloat("_Blood_Value", 0);

        if (PlayerPrefs.GetInt("Restart Hosptal After Camping", 0) == 0)
        {
            player.col.enabled = false;
        }
        else
        {
            player.anim.ResetAnim();
            player.transform.rotation = Quaternion.identity;
            nurse.gameObject.SetActive(false);
            player.anim.SpriteRenderer.sortingOrder = 7;
            player.SetPositon(Mai.transform.position + Vector3.up * 0.7f, Vector2.down);
        }

        yield return new WaitForSeconds(1);

        if (PlayerPrefs.GetInt("Restart Hosptal After Camping", 0) == 0)
        {
            nurse.Move(nursePath.paths, 5, NurseCommunicateWithPlayer);
        }
        else
        {
            KillMai();
        }
    }

    private void NurseCommunicateWithPlayer()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.dialoguesWithNurse, NurseMoveOut);
        nurse.anim.SetDirection(Vector2.down);
    }

    private void NurseMoveOut()
    {
        nurse.Move(nursePath.Reverse(), 5, () =>
        {
            nurse.gameObject.SetActive(false);
            PlayerWakeUp();
        });
    }

    private void PlayerWakeUp()
    {
        player.transform.DOMoveY(player.transform.position.y + 1, 1);
        player.transform.DORotate(Vector3.zero, 1);
        player.col.enabled = true;
        player.anim.ResetAnim();
        DOVirtual.DelayedCall(1, () =>
        {
            EnablePlayerMove();
            player.anim.SpriteRenderer.sortingOrder = 7;
        });
    }

    public void MaiTalkWithPlayer(InteractableEntity mai)
    {
        if (mai != null)
            mai.canInteract = false;
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.dialoguesWithMai, KillMai);
    }

    public void KillMai()
    {
        GameManager.instance.transitions.Transition(1, 1, EscapeFromHospital, () =>
        {
            MaiBlood.gameObject.SetActive(true);
            Mai.anim.SpriteRenderer.color = Color.red;
            player.anim.SpriteRenderer.material.SetFloat("_Blood_Value", .8f);
        });
    }

    private void EscapeFromHospital()
    {
        toEntrance.canInteract = true;
        GameManager.instance.dialogueManager.StartDialogue(texts.needToEscape, () =>
        {
            EnablePlayerMove();
            player.playerMovement.SetSpeed(8);
            StartClock();
        });
    }

    private void EnablePlayerMove()
    {
        player.EnableMoveAndUI();
        player.stress.HideStressBar();
        player.stress.HideBreathButton();
    }

    private void StartClock()
    {
        clock.transform.parent.gameObject.SetActive(true);
        StartCoroutine(StartCountdown());
    }

    bool isEndClock;
    IEnumerator StartCountdown()
    {
        float currentTime = escapeDuration;

        while (currentTime > 0f)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);

            clock.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return new WaitForSeconds(1f);

            currentTime -= 1f;
        }

        isEndClock = true;
        clock.text = "00:00";
        EscapeFailed();
    }

    private void EscapeFailed()
    {
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.stopPlayer, () =>
        {
            failBoard.SetActive(true);
            failedText.text = texts.playerFailedToEscape;
        });
    }

    public void Restart()
    {
        PlayerPrefs.SetInt("Restart Hosptal After Camping", 1);
        GameManager.instance.transitions.Transition(1, 1, null, () => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }

    public void EscapeSuccessfully(InteractableEntity entity)
    {
        if (isEndClock) return;
        StopAllCoroutines();
        GameManager.instance.dialogueManager.StartDialogue(texts.escapeSuccessfully, ToNextScene);
    }

    private void ToNextScene()
    {
        PlayerPrefs.SetInt("Progress", (int)GameProgress.FightWithNam);
        GameManager.instance.dbManager.UpdateDB();
        GameManager.instance.transitions.Transition(1, 1, null, ()=>SceneManager.LoadScene("FightWithNam"));
    }
}
