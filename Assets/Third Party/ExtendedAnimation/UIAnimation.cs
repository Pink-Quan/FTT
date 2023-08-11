using DG.Tweening;
using ExtendedAnimation;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ExtendedAnimation
{
    public class UIAnimation : MonoBehaviour
    {
        public bool playOnEnable;
        [Header("Time")]
        public float duration = 0.3f;
        public float bouchDuration = 0.15f;

        [Header("Move Effect")]
        public Vector2 moveOffset = new Vector2(0f, 200f);
        public Vector2 moveBouchOffset = new Vector2(0, -10f);

        [Header("Rotate Effect")]
        public Vector3 rotOffset;
        public Vector3 rotBouchOffset;

        [Header("Scale Effect")]
        [SerializeField] private Vector3 scaleOffset = new Vector3(-0.8f, -0.8f, -0.8f);
        [SerializeField] private Vector3 scaleBouchOffset = new Vector3(0.2f, 0.2f, 0.2f);

        protected Vector2 defaultPos;
        protected Vector3 defaultRot;
        protected Vector3 defaultScale;

        private RectTransform rectTransform;

        protected virtual void Awake()
        {
            GetItsRectTransform();

            defaultPos = rectTransform.anchoredPosition;
            defaultRot = rectTransform.localRotation.eulerAngles;
            defaultScale = rectTransform.localScale;
        }

        public void GetItsRectTransform()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        protected virtual void OnEnable()
        {
            if (playOnEnable)
                Show();
        }

        public void Show()
        {
            StopAllCoroutines();
            gameObject.SetActive(true);
            //Move effect
            rectTransform.anchoredPosition = defaultPos + moveOffset;
            rectTransform.DOAnchorPos(defaultPos + moveBouchOffset, duration).OnComplete(() =>
            {
                rectTransform.DOAnchorPos(defaultPos, bouchDuration).SetUpdate(UpdateType.Normal, true);
            }).SetUpdate(UpdateType.Normal, true);

            //Rotate effect
            transform.localRotation = Quaternion.Euler(defaultRot + rotOffset);
            transform.DORotateQuaternion(Quaternion.Euler(defaultRot + rotBouchOffset), duration).OnComplete(() =>
            {
                transform.DORotateQuaternion(Quaternion.Euler(defaultRot), bouchDuration).SetUpdate(UpdateType.Normal, true);
            }).SetUpdate(UpdateType.Normal, true);

            //Scale effect
            transform.localScale = defaultScale + scaleOffset;
            transform.DOScale(defaultScale + scaleBouchOffset, duration).OnComplete(() =>
            {
                transform.DOScale(defaultScale, bouchDuration).SetUpdate(UpdateType.Normal, true);
            }).SetUpdate(UpdateType.Normal, true);
        }

        public void Hide()
        {
            StartCoroutine(DisableUI(duration + bouchDuration));
            //Move effect
            rectTransform.DOAnchorPos(defaultPos + moveBouchOffset, bouchDuration).OnComplete(() =>
            {
                rectTransform.DOAnchorPos(defaultPos + moveOffset, duration).SetUpdate(UpdateType.Normal, true);
            }).SetUpdate(UpdateType.Normal, true);

            //Rotate effect
            transform.DORotateQuaternion(Quaternion.Euler(defaultRot + rotBouchOffset), bouchDuration).OnComplete(() =>
            {
                transform.DORotateQuaternion(Quaternion.Euler(defaultRot + rotOffset), duration).SetUpdate(UpdateType.Normal, true);
            }).SetUpdate(UpdateType.Normal, true);

            //Scale effect
            transform.DOScale(defaultScale + scaleBouchOffset, bouchDuration).OnComplete(() =>
            {
                transform.DOScale(defaultScale + scaleOffset, duration).SetUpdate(UpdateType.Normal, true);
            }).SetUpdate(UpdateType.Normal, true);
        }
        private IEnumerator DisableUI(float waitTime)
        {
            yield return new WaitForSecondsRealtime(waitTime);
            gameObject.SetActive(false);
        }
    }
}

