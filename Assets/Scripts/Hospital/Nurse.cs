using DG.Tweening;
using System.Collections;
using System;
using UnityEngine;

public class Nurse : MonoBehaviour
{
    public CharacterController controller;
    [SerializeField] private float appearDuration;
    [SerializeField] private float leaveDuration=3;

    [SerializeField] private VectorPaths appearPaths;
    [SerializeField] private VectorPaths leaveRoomPaths;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void ReachPlayer(Action OnReached)
    {
        controller.UpdateMoveAnimation();
        transform.DOPath(appearPaths.paths, appearDuration).OnComplete(()=>
        {
            OnReached?.Invoke();
            controller.StopMove();
            controller.anim.SetDirection(new Vector2(0,-1));
            StopAllCoroutines();
        });
    }

    public void Disappeare(Action OnDisappear)
    {
        controller.UpdateMoveAnimation();
        Array.Reverse(appearPaths.paths);
        transform.DOPath(appearPaths.paths, appearDuration).OnComplete(() =>
        {
            OnDisappear?.Invoke();
            controller.StopMove();
            controller.anim.SetDirection(new Vector2(0, -1));
            StopAllCoroutines();
        });
    }

    public void LeaveRoom(Action OnLeaveRoomDone)
    {
        controller.UpdateMoveAnimation();
        transform.DOPath(leaveRoomPaths.paths, leaveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            OnLeaveRoomDone?.Invoke();
            controller.anim.SetMove(false);
            controller.anim.SetDirection(new Vector2(0, -1));
            StopAllCoroutines();
            gameObject.SetActive(false);
        });
    }
}
