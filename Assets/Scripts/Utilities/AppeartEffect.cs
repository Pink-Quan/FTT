using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppeartEffect : MonoBehaviour
{
    [SerializeField] float duration;
    private void OnEnable()
    {
        Vector3 baseScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(baseScale, duration).SetEase(Ease.OutBounce);
    }
}
