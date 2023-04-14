using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tes : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public int spriteIndex;

    private void Update()
    {
        spriteRenderer.sprite = sprites[spriteIndex];
    }

}
