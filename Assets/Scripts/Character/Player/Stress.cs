using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stress : MonoBehaviour
{
    [Range(0, 100)]
    public int stressIndex;
    public Image stressBar;
    public PlayerController player;
    public Breath breath;

    public void AddStress(int add)
    {
        stressIndex += add;
        stressIndex = Mathf.Clamp(stressIndex, 0, 100);
        if(stressIndex <20)
        {
            player.playerMovement.ResetPlayerSpeed();
            stressBar.color = Color.green;
        }    
        else if(stressIndex >20 && stressIndex <50)
        {
            player.playerMovement.SetSpeed(player.playerMovement.DefaultSpeed / 2);
            stressBar.color = Color.yellow;

        }    
        else if(stressIndex >=50 && stressIndex < 70)
        {
            player.playerMovement.SetSpeed(player.playerMovement.DefaultSpeed / 3);
            stressBar.color= new Color(1, 69f /255, 0,1);
        }
        else
        {
            player.playerMovement.SetSpeed(0);
            stressBar.color = Color.red;
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
}
