using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Stress : MonoBehaviour
{
    [Range(0, 100)]
    public int stressIndex;
    public Image stressBar;
    public PlayerController player;
    public Breath breath;

    public UnityEvent onMaxStress;

    private void Start()
    {
        SetBarColor();
    }

    public void AddStress(int add)
    {
        stressIndex += add*10;
        SetBarColor();
    }

    public void SetStress(int value)
    {
        stressIndex = value;
        SetBarColor();
    }
    bool isReachedMaxStress;
    private void SetBarColor()
    {
        stressIndex = Mathf.Clamp(stressIndex, 0, 100);
        if (stressIndex < 20)
        {
            player.playerMovement.ResetPlayerSpeed();
            stressBar.color = Color.green;
            isReachedMaxStress = false;
        }
        else if (stressIndex > 20 && stressIndex < 50)
        {
            player.playerMovement.SetSpeed(player.playerMovement.DefaultSpeed / 2f);
            stressBar.color = Color.yellow;
            isReachedMaxStress = false;
        }
        else if (stressIndex >= 50 && stressIndex < 80)
        {
            player.playerMovement.SetSpeed(player.playerMovement.DefaultSpeed / 3f);
            stressBar.color = new Color(1, 69f / 255, 0, 1);
            isReachedMaxStress = false;
        }
        else
        {
            player.playerMovement.SetSpeed(0);
            stressBar.color = Color.red;
            if (!isReachedMaxStress)
                onMaxStress?.Invoke();
            isReachedMaxStress = true;
        }
        stressBar.fillAmount = stressIndex / 100f;
    }


    public void ReduceStrees()
    {
        breath.StartBreath((succesBreath) =>
        {
            succesBreath = -succesBreath;
            AddStress(succesBreath);
        },10);
    }

    public void HideStressBar()
    {
        stressBar.transform.parent.gameObject.SetActive(false);
    }

    public void ShowStressBar()
    {
        stressBar.transform.parent.gameObject.SetActive(true);
    }

    public void ShowBreathButton()
    {
        breath.breathButton.gameObject.SetActive(true);
    }

    public void HideBreathButton()
    {
        breath.breathButton.gameObject.SetActive(false);
    }

    public void StartStress()
    {
        StopAllCoroutines();
        StartCoroutine(BeingMoreStress());
    }

    public void StopBeingStress()
    {
        StopAllCoroutines();
    }

    private IEnumerator BeingMoreStress(int addRate = 1)
    {
        while(true)
        {
            yield return new WaitForSeconds(5);
            AddStress(addRate);
        }
    }
}
