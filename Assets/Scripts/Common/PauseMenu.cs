using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = Mathf.Clamp01(timeScale);
    }
}
