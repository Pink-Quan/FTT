using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class MainMenuBG : MonoBehaviour
{
    [SerializeField] private float frequency;
    [SerializeField] private float amplitude;
    [SerializeField] private RectTransform rectTransform;

    private void Start()
    {
        rectTransform.anchoredPosition = Vector3.zero;
    }

    private void Update()
    {
        Vector2 tOffset = rectTransform.anchoredPosition;
        tOffset.x += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        rectTransform.anchoredPosition = tOffset;
    }
}
