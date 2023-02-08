using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExtendedAnimation
{
    public class UIAnimation : MonoBehaviour
    {
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
        [SerializeField] private Vector3 scaleOffset=new Vector3(-0.8f,-0.8f,-0.8f);
        [SerializeField] private Vector3 scaleBouchOffset=new Vector3(0.2f,0.2f,0.2f);

        private Vector2 defaultPos;
        private Vector3 defaultRot;
        private Vector3 defaultSca;

        protected virtual void Awake()
        {
            defaultPos = transform.localPosition;
            defaultRot = transform.localRotation.eulerAngles;
            defaultSca= transform.localScale;
        }

        protected virtual void OnEnable()
        {
            Show();
        }

        protected void Show()
        {
            //Move effect
            transform.localPosition = defaultPos + moveOffset;
            transform.DOLocalMove(defaultPos+moveBouchOffset, duration).OnComplete(() =>
            {
                transform.DOLocalMove(defaultPos, bouchDuration);
            });

            //Rotate effect
            transform.localRotation = Quaternion.Euler(defaultRot+rotOffset);
            transform.DORotateQuaternion(Quaternion.Euler(defaultRot+rotBouchOffset), duration).OnComplete(() =>
            {
                transform.DORotateQuaternion(Quaternion.Euler(defaultRot), bouchDuration);
            });

            //Scale effect
            transform.localScale = defaultSca + scaleOffset;
            transform.DOScale(defaultSca+scaleBouchOffset, duration).OnComplete(() =>
            {
                transform.DOScale(defaultSca, bouchDuration);
            });
        }

        public void Hide()
        {
            Invoke("DisableUI", duration+bouchDuration);
            //Move effect
            transform.DOLocalMove(defaultPos + moveBouchOffset, bouchDuration).OnComplete(() =>
            {
                transform.DOLocalMove(defaultPos + moveOffset, duration);
            });

            //Rotate effect
            transform.DORotateQuaternion(Quaternion.Euler(defaultRot + rotBouchOffset), bouchDuration).OnComplete(() =>
            {
                transform.DORotateQuaternion(Quaternion.Euler(defaultRot + rotOffset), duration);
            });

            //Scale effect
            transform.DOScale(defaultSca + scaleBouchOffset, bouchDuration).OnComplete(() =>
            {
                transform.DOScale(defaultSca + scaleOffset, duration);
            });
        }
        private void DisableUI()
        {
            gameObject.SetActive(false);
        }
    }
}


