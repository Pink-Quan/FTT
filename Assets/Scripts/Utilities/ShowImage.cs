using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShowImage : MonoBehaviour
{
    public Image image;

    public UnityAction onClose;
    public void Show(Sprite img,UnityAction onClose = null)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        image.sprite = img;
        this.onClose = onClose;
    }

    public void OnCloseViewImage()
    {
        onClose?.Invoke();
    }
}
