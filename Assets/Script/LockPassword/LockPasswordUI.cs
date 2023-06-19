using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LockPasswordUI : MonoBehaviour
{
    [SerializeField] private string password;   
    [SerializeField] private InputNumberUI[] inputs;

    public UnityEvent OnUncloked;
    public void CheckPassword()
    {
        string input = "";
        for (int i = 0; i < inputs.Length; i++)
            input += inputs[i].number.ToString();

        if (string.Compare(input, password)==0)
            OnUncloked?.Invoke();
    }
}
