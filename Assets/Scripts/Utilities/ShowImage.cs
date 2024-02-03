using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShowImage : MonoBehaviour
{
    public Image image;

    public UnityAction onClose;
    public void Show(Sprite img, UnityAction onClose = null)
    {
        gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
        image.sprite = img;
        this.onClose = onClose;
    }

    public void OnCloseViewImage()
    {
        onClose?.Invoke();
        gameObject.SetActive(false);
    }

    public void ResizeImage()
    {
        var rectTrans = transform as RectTransform;

        float screenWidth = rectTrans.rect.width;
        float screenHeight = rectTrans.rect.height;

        float imageWidth = image.sprite.bounds.size.x;
        float imageHeight = image.sprite.bounds.size.y;

        float scaleFactorWidth = screenWidth / imageWidth;
        float scaleFactorHeight = screenHeight / imageHeight;

        float scaleFactor = Mathf.Min(scaleFactorWidth, scaleFactorHeight);

        RectTransform rectTransform = image.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(imageWidth * scaleFactor, imageHeight * scaleFactor);
        }

    }

}
