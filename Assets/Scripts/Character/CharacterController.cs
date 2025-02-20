using DG.Tweening;
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
    Vector2 lastDir = Vector2.right;
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
            else
            {
                dir = lastDir;
            }
            anim.SetDirection(dir);
            lastPos = transform.position;
            lastDir = dir;
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

    public virtual void ImmediateDie(float fallDuration = 0.2f, TweenCallback onDone = null)
    {
        anim.StopAnimation();
        anim.SpriteRenderer.color = Color.red;
        transform.DORotate(new Vector3(0, 0, 90), fallDuration).OnComplete(onDone);
    }

    public void Resurrect()
    {
        transform.rotation = Quaternion.identity;
        anim.ResetAnim();
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

    public void SetPositon(Vector3 pos, Vector2 dir)
    {
        transform.position = pos;
        anim.SetDirection(dir);
    }

    private void OnPunch()
    {

    }

    private void OnPunchDone()
    {

    }

    public void Move(Vector3[] paths, float speed, TweenCallback onDone = null)
    {
        UpdateMoveAnimation();
        onDone += StopMove;

        transform.DOPath(paths, GetDistance(paths) / speed).OnComplete(onDone).SetEase(Ease.Linear);
    }

    private float GetDistance(Vector3[] paths)
    {
        paths[0].z = transform.position.z;
        float res = 0;
        res += Vector3.Distance(paths[0], transform.position);
        if (paths.Length > 1)
        {
            for (int i = 0; i < paths.Length - 1; i++)
            {
                paths[i].z = 0;
                paths[i + 1].z = 0;
                res += Vector3.Distance(paths[i], paths[i + 1]);
            }
        }
        return res;
    }
}
