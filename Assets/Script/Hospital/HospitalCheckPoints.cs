using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class HospitalCheckPoints : MonoBehaviour
{
    [SerializeField] private Transform[] checkPoints;
    [SerializeField] private float radius;
    [SerializeField] private bool isGizmos;
    [SerializeField] private Transform player;

    [DefaultValue(0)]
    public int checkingIndex
    {
        get;
        private set;
    }

    public Action<Transform> OnDoneOneCheckPoint;
    public Action OnDoneAllCheckPoint;

    private void Update()
    {
        if (checkingIndex == checkPoints.Length) Destroy(gameObject);

        if (((Vector2)(player.transform.position - checkPoints[checkingIndex].position)).sqrMagnitude <= radius * radius)
        {
            checkPoints[checkingIndex].gameObject.SetActive(false);
            checkingIndex++;
            if (checkingIndex == checkPoints.Length) 
            {
                OnDoneAllCheckPoint?.Invoke();
                Destroy(gameObject);
                return;
            }
            checkPoints[checkingIndex].gameObject.SetActive(true);
            OnDoneOneCheckPoint?.Invoke(checkPoints[checkingIndex]);
        }
    }
    public Transform[] GetCheckPoints() => checkPoints;

    private void OnDrawGizmos()
    {
        if (!isGizmos) return;

        for (int i = 0; i < checkPoints.Length; i++)
        {
            Gizmos.DrawWireSphere(checkPoints[i].position, radius);
            Gizmos.DrawLine(checkPoints[i].position, checkPoints[(i+1) % checkPoints.Length ].position);
        }
    }
}
