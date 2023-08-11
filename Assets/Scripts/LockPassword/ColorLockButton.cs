using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ColorLockButton : MonoBehaviour
{
    public int currentIndex;
    int maxIndex;
    Button button;
    Color[] colors;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ModifyColorIndex);
    }

    public void Init(Color[] colors)
    {
        this.colors = colors;
        maxIndex = colors.Length;
    }
    
    private void ModifyColorIndex()
    {
        currentIndex++;
        currentIndex %= maxIndex;
        button.image.color = colors[currentIndex];
    }

}
