using DG.Tweening;
using System.Collections;
using System;
using UnityEngine;

public class Nurse : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private float appearDuration;

    [SerializeField] private VectorPaths appearPaths;

    private void Start()
    {
        lastPos = transform.position;
        controller = GetComponent<CharacterController>();
    }

    public void ReachPlayer(Action OnReached)
    {
        StartCoroutine(UpdateMove());
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
        StartCoroutine(UpdateMove());
        Array.Reverse(appearPaths.paths);
        transform.DOPath(appearPaths.paths, appearDuration).OnComplete(() =>
        {
            OnDisappear?.Invoke();
            controller.anim.SetMove(false);
            controller.anim.SetDirection(new Vector2(0, -1));
            StopAllCoroutines();
        });
    }

    Vector3 lastPos;
    private IEnumerator UpdateMove()
    {
        while (true)
        {
            Vector2 dir = transform.position - lastPos;
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)){
                dir.y = 0;
                if (dir.x != 0) dir.x /= Mathf.Abs(dir.x);
            }
            else if(Mathf.Abs(dir.x) < Mathf.Abs(dir.y))
            {
                dir.x = 0;
                if (dir.y != 0) dir.y /= Mathf.Abs(dir.y);
            }
            controller.anim.SetDirection(dir);
            lastPos = transform.position;
            controller.anim.SetMove(true);

            yield return new WaitForFixedUpdate();
        }
    }
}
