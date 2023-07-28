using UnityEngine;
using System.Collections;
using TMPro;

public class Fps : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsText;

    private IEnumerator Start()
    {
        while (true)
        {
            fpsText.text = ((int)(1f / Time.unscaledDeltaTime)).ToString()+" FPS";
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
