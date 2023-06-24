using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SimpleBoxCollider))]
public class SimpleBoxColliderTargetEvent : MonoBehaviour
{
    public Transform target;

    public UnityEvent onTargetEnter;
    public UnityEvent onTargeStay;
    public UnityEvent onTargetLeave;

    private SimpleBoxCollider col;

    private void Awake()
    {
        col = GetComponent<SimpleBoxCollider>();
    }

    bool isEnter;
    private void Update()
    {
        if(target==null) return;

        if (col.CheckPoint(target.position))
        {
            if (!isEnter)
            {
                onTargetEnter?.Invoke();
                isEnter = true;
            }
            onTargeStay?.Invoke();
        }
        else if(isEnter)
        {
            onTargetLeave?.Invoke();
            isEnter = false;
        }
    }

    public void SetEnter(bool isEnter)
    {
        this.isEnter = isEnter;
    }

}
