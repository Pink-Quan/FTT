using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : CharacterController
{
    public PlayerMovement playerMovement;
    public Rigidbody2D rb;
    public Inventory inventory;
    public ArrowPointer arrowPointer;
    public GameObject buttons;
    public Button interactButton;
    public Button inventoryButton;
    public Canvas canvas;
    public Button currentItemButton;

    public Item curItem;

    public void SetArrowPointer(Transform target)
    {
        arrowPointer.gameObject.SetActive(true);
        arrowPointer.target = target;
    }

    public void OffArrowPointer()
    {
        arrowPointer.target = null;
        arrowPointer.gameObject.SetActive(false);
    }

    public void ShowItertactButton(UnityEvent OnInteract, string interactName)
    {
        interactButton.gameObject.SetActive(true);
        interactButton.onClick.AddListener(() => OnInteract?.Invoke());
        interactButton.GetComponentInChildren<TMP_Text>().text = interactName;
    }

    public void HideInteractButton()
    {
        interactButton.onClick.RemoveAllListeners();
        interactButton.gameObject.SetActive(false);
    }

    Image currentItemButtonImage;
    public void SelectItem(Item item)
    {
        if (currentItemButtonImage == null) currentItemButtonImage = currentItemButton.GetComponent<Image>();
        if (item.icon == null) 
            currentItemButtonImage.color = new Color(0, 0, 0, 0);
        else
        {
            currentItemButtonImage.sprite = item.icon;
            curItem = item;
        }
    }
}
