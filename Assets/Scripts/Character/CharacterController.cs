using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CharacterController : MonoBehaviour
{
    public CharacterAnim anim;
    public Animator animator;
    public Collider2D col;
    public CharacterSound sound;
    public InteractableEntity interact;
    protected virtual void Start()
    {
        lastPos = transform.position;
    }


    Vector3 lastPos;
    public IEnumerator StartUpdateMoveAnimation()
    {
        while (true)
        {
            Vector2 dir = transform.position - lastPos;
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                dir.y = 0;
                if (dir.x != 0) dir.x /= Mathf.Abs(dir.x);
            }
            else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y))
            {
                dir.x = 0;
                if (dir.y != 0) dir.y /= Mathf.Abs(dir.y);
            }
            anim.SetDirection(dir);
            lastPos = transform.position;
            anim.SetMove(true);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    public void UpdateMoveAnimation()
    {
        StartCoroutine(StartUpdateMoveAnimation());
    }

    public void StopMove()
    {
        StopAllCoroutines();
        anim.SetMove(false);
    }

    public virtual void Die()
    {
        anim.Die();
    }
    public void AddConversationToCharacter(Dialogue dialogue, Action onDone = null, Action onInteract = null)
    {
        interact.canInteract = true;
        interact.onInteract.RemoveAllListeners();
        interact.onInteract.AddListener(TalkWithCharacter);

        void TalkWithCharacter(InteractableEntity entity)
        {
            GameManager.instance.DisablePlayerMoveAndUI();
            GameManager.instance.dialogueManager.StartDialogue(dialogue, DoneTalkWithCharacter);

            void DoneTalkWithCharacter()
            {
                GameManager.instance.EnablePlayerMoveAndUI();
                GameManager.instance.player.ShowInteractButton();
                onDone?.Invoke();
            }
        }
    }

    public void ClearConversation()
    {
        interact.canInteract = false;
        interact.onInteract.RemoveAllListeners();
    }

    public void AddConversationToCharacter(Dialogue[] dialogue, Action onDone = null, Action onInteract = null)
    {
        interact.canInteract = true;
        interact.onInteract.RemoveAllListeners();
        interact.onInteract.AddListener(TalkWithCharacter);

        void TalkWithCharacter(InteractableEntity entity)
        {
            GameManager.instance.DisablePlayerMoveAndUI();
            GameManager.instance.dialogueManager.StartDialogue(dialogue, DoneTalkWithCharacter);
            onInteract?.Invoke();

            void DoneTalkWithCharacter()
            {
                GameManager.instance.EnablePlayerMoveAndUI();
                GameManager.instance.player.ShowInteractButton();
                onDone?.Invoke();
            }
        }
    }

    public void SetPositon(Vector3 pos,Vector2 dir)
    {
        transform.position = pos;
        anim.SetDirection(dir);
    }
}
