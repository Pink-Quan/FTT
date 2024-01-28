using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    public Button missionButton;
    public GameObject joystick;
    public Canvas canvas;
    public Button currentItemButton;
    public SmartPhone phone;
    public Stress stress;

    public Item curItem;

    private void Awake()
    {
        GameManager.instance.player = this;
    }

    protected override void Start()
    {
        base.Start();
        canvas.worldCamera = Camera.main;
        if (SystemInfo.deviceType == DeviceType.Handheld) joystick.SetActive(true);
        else joystick.SetActive(false);
    }

    public void EnableMove()
    {
        playerMovement.enabled = true;
        buttons.SetActive(true);
        if(SystemInfo.deviceType==DeviceType.Handheld) joystick.SetActive(true);
    }

    public void DisableMove()
    {
        anim.SetMove(false);
        playerMovement.enabled = false;
        joystick.SetActive(false);
    }

    public void HideButtons()
    {
        buttons.SetActive(false);
        joystick.SetActive(false);
    }

    public void ShowButtons()
    {
        buttons.SetActive(true);
        if (SystemInfo.deviceType == DeviceType.Handheld) joystick.SetActive(true);
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

    public void ShowInteractButton(InteractableEntity.OnInteractEntity OnInteract, string interactName, InteractableEntity entity)
    {
        interactButton.gameObject.SetActive(true);
        interactButton.onClick.RemoveAllListeners();
        interactButton.onClick.AddListener(() => OnInteract?.Invoke(entity));
        interactButton.GetComponentInChildren<TMP_Text>().text = interactName;
    }

    public void ShowInteractButton()
    {
        if(!interactButton.gameObject.activeSelf) interactButton.gameObject.SetActive(true);
    }

    public void HideInteractButton()
    {
        if(interactButton==null) return;
        interactButton.gameObject.SetActive(false);
    }

    public void HideUI()
    {
        HideButtons();
        stress.HideStressBar();
    }

    public void ShowUI()
    {
        ShowButtons();
        stress.ShowStressBar();
    }

    public void DisableMoveAndUI()
    {
        HideUI();
        DisableMove();
    }

    public void EnableMoveAndUI()
    {
        EnableMove();
        ShowUI();
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
    public Action onOpenPhone;
    private void InitPhoneButton()
    {
        currentItemButton.onClick.AddListener(() =>
        {
            phone.gameObject.SetActive(true);
            onOpenPhone?.Invoke();
        });
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

    public void OpenMissions()
    {
        GameManager.instance.missionsManager.ShowMissions();
    }
}
