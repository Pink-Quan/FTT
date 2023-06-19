using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InputNumberUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private int maxInput;

    public int number;
    public UnityEvent OnValueChange;
    private void Start()
    {
        text.text = number.ToString();
    }

    public void IncreaseNumber(int count)
    {
        number = (number + count) % maxInput;
        if (number < 0) number = maxInput;
        text.text = number.ToString();
        OnValueChange?.Invoke();
    }

}
