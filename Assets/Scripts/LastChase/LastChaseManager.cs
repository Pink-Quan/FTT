using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class LastChaseManager : MonoBehaviour
{
    public PlayerController player;
    public CharacterController Linh;
    public Cinemachine.CinemachineVirtualCamera mainCam;

    [SerializeField] VectorPaths LinhMovePath;
    [SerializeField] GameObject mainMap;
    [SerializeField] GameObject dieMap;
    [SerializeField] Vector3 LinhDiePos = new Vector3(104.43f, -163.26f);
    [SerializeField] Light2D NamLight;
    [SerializeField] VectorPaths namReachLinhPath;

    private LastChaseTexts texts;
    void Start()
    {
        texts = Resources.Load<LastChaseTexts>($"Texts/LastChase/{PlayerPrefs.GetString("Language", "Eng")}");
        player.DisableMoveAndUI();

        player.anim.SetDirection(Vector2.left);
        Linh.anim.SetDirection(Vector2.right);

        player.transform.rotation = Quaternion.Euler(0, 0, 90);
        Linh.transform.rotation = Quaternion.Euler(0, 0, -90);

        player.transform.DORotate(Vector3.zero, 1);
        Linh.transform.DORotate(Vector3.zero, 1);
        DOVirtual.DelayedCall(1, () =>
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.firstDialogue, LinhTryToEscape);
        });
    }

    private void LinhTryToEscape()
    {
        mainCam.Follow = player.transform;
        GameManager.instance.textBoard.ShowText(texts.mission, () =>
        {
            player.EnableMoveAndUI();
            player.HideButtons();
            player.stress.HideStressBar();
            Linh.Move(LinhMovePath.paths, 5, () =>
            {
                Linh.anim.SetDirection(Vector2.down);
            });
        });
    }

    public void ReachLinh(Collision2D col, Collider2D caller)
    {
        mainCam.Follow = null;
        caller.enabled = false;
        caller.gameObject.SetActive(false);
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.namToldLinhToStop, LinhJump);
    }

    private void LinhJump()
    {
        mainCam.transform.DOMoveY(mainCam.transform.position.y - 10, 1).OnComplete(() =>
        {
            Linh.transform.position = new Vector3(mainCam.transform.position.x, Linh.transform.position.y);
            Linh.transform.DOMoveY(Linh.transform.position.y - 20, 4).OnComplete(LinhDeadScene);
        });
    }

    private void LinhDeadScene()
    {
        GameManager.instance.transitions.Transition(1, 1, NamReachLinhInDead, InitDead);
    }

    void InitDead()
    {
        mainMap.SetActive(false);
        dieMap.SetActive(true);
        Linh.transform.position = LinhDiePos;
        Linh.transform.rotation = Quaternion.Euler(0, 0, 90);
        mainCam.Follow = Linh.transform;
        Linh.anim.StopAnimation();
        Linh.anim.SpriteRenderer.color = Color.red;
    }

    private void NamReachLinhInDead()
    {
        player.DisableMoveAndUI();
        player.col.enabled = false;
        player.transform.position = namReachLinhPath.paths[0];
        player.Move(namReachLinhPath.paths, 5, LinhTellLastWord);
    }

    private void LinhTellLastWord()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.lastDialougueWithNam, EndLastChase);
    }

    private void EndLastChase()
    {
        player.anim.SpriteRenderer.DOFade(0, 2);
        DOVirtual.Float(NamLight.intensity, 0, 2, lightIdensity =>
        {
            NamLight.intensity = lightIdensity;
        });
        DOVirtual.DelayedCall(1, ToTheTruthScene);
    }

    private void ToTheTruthScene()
    {
        PlayerPrefs.SetInt("Progress", (int)GameProgress.TheTruth);
        GameManager.instance.dbManager.UpdateDB();
        print("End");
        GameManager.instance.transitions.Transition(1, 1, null, () => SceneManager.LoadScene("TheTruth"));
    }
}
