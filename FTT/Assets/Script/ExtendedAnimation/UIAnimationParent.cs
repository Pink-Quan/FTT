using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtendedAnimation
{
    public class UIAnimationParent : MonoBehaviour
    {
        private UIAnimation[] animArray;

        private void Awake()
        {
            animArray = GetComponentsInChildren<UIAnimation>();
        }

        private void OnEnable()
        {
            Show();
        }

        public void Show()
        {
            for (int i = 0; i < animArray.Length; i++)
                animArray[i].gameObject.SetActive(true);
        }
        public void Hide()
        {
            float latestTime = 0f;
            for (int i = 0; i < animArray.Length; i++)
            {
                if (latestTime < animArray[i].duration + animArray[i].bouchDuration)
                    latestTime = animArray[i].duration + animArray[i].bouchDuration;
                animArray[i].Hide();
            }
            Invoke("HideItseft", latestTime);
        }
        private void HideItseft()
        {
            gameObject.SetActive(false);
        }
    }
}


