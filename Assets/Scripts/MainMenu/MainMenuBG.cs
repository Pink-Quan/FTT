using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class MainMenuBG : MonoBehaviour
{
    [SerializeField] private float frequency;
    [SerializeField] private float amplitude;
    [SerializeField] private Material material;

    private void Start()
    {
        material.mainTextureOffset = Vector2.zero;
    }

    private void Update()
    {
        material.mainTextureOffset = Vector2.right*Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
    }
}
