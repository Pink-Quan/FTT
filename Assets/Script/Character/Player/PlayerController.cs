using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class PlayerController : CharacterController
{
    public PlayerMovement playerMovement;
    public Transform directionTrasform;
    public Rigidbody2D rb;
    public Inventory inventory;
    public ArrowPointer arrowPointer;
    public GameObject buttons;
    public Button interactButton;
    public Button inventoryButton;
    public Canvas canvas;
    public Button currentItemButton;
    public SmartPhone phone;

    public Item curItem;

    private void Awake()
    {
        GameManager.instance.player = this;
    }

    public void EnableMove()
    {
        playerMovement.enabled = true;
        buttons.SetActive(true);
    }

    public void DisableMove()
    {
        playerMovement.enabled = false;
    }

    public void HideButtons()
    {
        buttons.SetActive(false);
    }

    public void ShowButtons()
    {
        buttons.SetActive(true);
    }

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
        currentItemButton.onClick.RemoveAllListeners();
        switch (item.itemType)
        {
            case ItemType.NormalItem:
                if (string.Compare(item.itemName, "Phone") == 0)
                    InitPhoneButton();

                break;
        }
    }

    private void InitPhoneButton()
    {
        currentItemButton.onClick.AddListener(() => phone.gameObject.SetActive(true));
        currentItemButton.onClick.AddListener(DisableMove);
        currentItemButton.onClick.AddListener(HideButtons);
        phone.outButton.onClick.AddListener(EnableMove);
    }

    private GameObject flashLight;
    public void TurnFlashLight(bool isOn)
    {
        flashLight = phone.flashLight;
        if (isOn)
        {
            flashLight.SetActive(true);
            flashLight.transform.SetParent(directionTrasform);

            flashLight.transform.localPosition = Vector3.zero;
            flashLight.transform.localRotation = Quaternion.Euler(0, 0, 180);
            flashLight.transform.localScale = Vector3.one;
        }
        else
        {
            flashLight.SetActive(false);
        }
    }
    public bool IsFlashLight()
    {
        if (flashLight == null) return false;
        return flashLight.activeSelf;
    }

    public void OffInteractButton()
    {
        interactButton.gameObject.SetActive(false);
    }

    public void OnInteractButton()
    {
        interactButton.gameObject.SetActive(true);
    }

    public override void Die()
    {
        base.Die();
        DisableMove();
        HideButtons();
    }
}
