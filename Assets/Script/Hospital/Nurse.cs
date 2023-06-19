using DG.Tweening;
using System.Collections;
using System;
using UnityEngine;

public class Nurse : MonoBehaviour
{
    private CharacterController controller;
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
        StartCoroutine(controller.UpdateMove());
        transform.DOPath(appearPaths.paths, appearDuration).OnComplete(()=>
        {
            OnReached?.Invoke();
            controller.anim.SetMove(false);
            controller.anim.SetDirection(new Vector2(0,-1));
            StopAllCoroutines();
        });
    }

    public void Disappeare(Action OnDisappear)
    {
        StartCoroutine(controller.UpdateMove());
        Array.Reverse(appearPaths.paths);
        transform.DOPath(appearPaths.paths, appearDuration).OnComplete(() =>
        {
            OnDisappear?.Invoke();
            controller.anim.SetMove(false);
            controller.anim.SetDirection(new Vector2(0, -1));
            StopAllCoroutines();
        });
    }

    public void LeaveRoom(Action OnLeaveRoomDone)
    {
        StartCoroutine(controller.UpdateMove());
        transform.DOPath(leaveRoomPaths.paths, leaveDuration).OnComplete(() =>
        {
            OnLeaveRoomDone?.Invoke();
            controller.anim.SetMove(false);
            controller.anim.SetDirection(new Vector2(0, -1));
            StopAllCoroutines();
            gameObject.SetActive(false);
        });
    }
}
