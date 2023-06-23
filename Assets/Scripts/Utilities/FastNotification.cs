using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FastNotification : MonoBehaviour
{
    [SerializeField] TMP_Text notificationPrefab;
    [SerializeField] float notificationDuration=2;
    [SerializeField] float notificationHeight=2;

    public void Show(Vector3 position,string text)
    {
        var t = SimplePool.Spawn(notificationPrefab);
        t.transform.SetParent(transform, false);

        t.transform.position = position;
        t.transform.rotation = Quaternion.identity;
        t.transform.localScale = Vector3.one;

        t.text = text;

        t.transform.DOMoveY(t.transform.position.y + notificationHeight, notificationDuration).
            OnComplete(()=>SimplePool.Despawn(t.gameObject)).SetEase(Ease.Linear);
    }
}
