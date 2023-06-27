using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClosetPassword : MonoBehaviour
{
    [SerializeField] private int password;
    [SerializeField] private Button[] buttons;

    public UnityEvent OnUnlock;

    private bool[] input;
    private bool[] pass;
    private int length;
    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int t = i;
            buttons[i].onClick.AddListener(() => { SwitchButton(t); });
        }
        length=buttons.Length;
        input = new bool[length];
        pass = new bool[length];

        while (password > 0)
        {
            pass[password % 10] = true;
            password /= 10;
        }
    }

    public void SwitchButton(int i)
    {
        //Debug.Log(i);
        input[i]=buttons[i].GetComponent<SwitchButton>().isOn;

        //string inp="";
        //string pa="";

        //for (int j = 0; j < length; j++)
        //{
        //    if (input[j])
        //        inp += $"{j}";
        //    if (pass[j])
        //        pa += $"{j}";
        //}

        //Debug.Log("INPUT="+inp);
        //Debug.Log("PASSWORD="+pa);

        for (int j = 0; j < length; j++)
            if (input[j] != pass[j])
                return;
        OnUnlock?.Invoke();
        Debug.Log("Password correct");
    }
}
