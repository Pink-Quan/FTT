using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]

public class FitSpriteRendererWithMainCamera : MonoBehaviour
{
    private Camera mainCam;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        mainCam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        FitWithMainCam();
    }

    public void FitWithMainCam()
    {
        transform.localScale= Vector3.one;
        Vector2 camSize = new Vector2(mainCam.orthographicSize * mainCam.aspect, mainCam.orthographicSize) * 2f;
        Vector2 rendererSize = spriteRenderer.bounds.size;

        transform.position = new Vector3(mainCam.transform.position.x,mainCam.transform.position.y,transform.position.z);

        float multiScale = Mathf.Max(camSize.x / rendererSize.x, camSize.y / rendererSize.y);
        transform.localScale = Vector3.one * multiScale;    
    }
}
