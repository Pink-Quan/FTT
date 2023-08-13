using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Missions : MonoBehaviour
{
    public List<string> missions = new List<string>();

    public void ShowMissions()
    {
        StringBuilder text = new StringBuilder();
        text.Append("Missions: ");
        text.Append('\n');
        foreach (string s in missions)
        {
            text.Append(s);
            text.Append('\n');
        }
        if (text.Length > 0)
            GameManager.instance.textBoard.ShowText(text.ToString());
    }

    public void ShowMissions(UnityAction onOK)
    {
        StringBuilder text = new StringBuilder();
        text.Append("Missions: ");
        text.Append('\n');
        foreach (string s in missions)
        {
            text.Append(s);
            text.Append('\n');
        }
        if (text.Length > 0)
            GameManager.instance.textBoard.ShowText(text.ToString(),onOK);
    }
}
