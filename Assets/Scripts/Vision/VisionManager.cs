using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionManager : MonoBehaviour
{
    public VisionZone currentVision;

    public static VisionManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentVision.GetComponent<SimpleBoxColliderTargetEvent>().SetEnter(true);
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
