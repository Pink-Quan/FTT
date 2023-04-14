using System.Security.AccessControl;
using System.Numerics;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nurse : MonoBehaviour
{
    [SerializeField] private float speed=5;

    [SerializeField] private Tranform[] appearPaths;
    [SerializeField] private Tranform[] disappearPaths;

    public void AppearAndTalkToPlayer()
    {

    }

    private void MoveTargetTo(Tranform target,Vector3 to,System.Action OnComplete){
        Vector3 basePos=target.position;
    }

    IEmutator StartMove(){
        
    }
}
