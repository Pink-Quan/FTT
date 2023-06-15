using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    public PlayerMovement playerMovement;
    public Rigidbody2D rb;
    public Inventory inventory;
    public GameObject openInventoryButton;
    public ArrowPointer arrowPointer;

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
}
