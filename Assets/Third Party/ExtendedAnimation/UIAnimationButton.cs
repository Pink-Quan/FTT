using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ExtendedAnimation
{
    [RequireComponent(typeof(Button))]
    public class UIAnimationButton : UIAnimation
    {
        [Header("Button Effect")]
        [SerializeField] private float bouchOffset = 0.2f;
        [SerializeField] private float bouchButtonDuration = 0.05f;

        private Button button;
        protected override void Awake()
        {
            base.Awake();
            button = GetComponent<Button>();
        }
        private void Start()
        {
            button.onClick.AddListener(ButtonEffect);
        }
        private void ButtonEffect()
        {
            transform.DOScale(defaultScale - defaultScale * bouchOffset, bouchButtonDuration).OnComplete(() =>
            {
                transform.DOScale(defaultScale, bouchOffset);
            });
        }
    }
}
