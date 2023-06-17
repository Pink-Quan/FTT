using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5f;

    private Vector2 moveDirection;
    private Rigidbody2D thisRigidbody2D;
    private PlayerController playerController;
    private CharacterAnim anim;
    private InputAction moveInput;

    private void Awake()
    {
        GetComponentInit();
    }

    private void OnEnable()
    {
        moveInput = GameManager.instance.input.Player.Move;
        moveInput.Enable();

    }
    private void OnDisable()
    {
        moveInput.Disable();
    }

    private void GetComponentInit()
    {
        playerController = GetComponent<PlayerController>();
        thisRigidbody2D = playerController.rb;
        anim = playerController.anim;
    }    

    private void FixedUpdate()
    {
        moveDirection = Vector2.zero;
        bool isMoving = false;
        //if (Input.GetKey(leftButton))  { moveDirection.x = -1; isMoving = true; }
        //if (Input.GetKey(rightButton)) { moveDirection.x = 1; isMoving = true;  }
        //if (Input.GetKey(upButton))    { moveDirection.y = 1; isMoving = true;  }
        //if (Input.GetKey(downButton))  { moveDirection.y = -1; isMoving = true; }

        moveDirection = moveInput.ReadValue<Vector2>();
        if (moveDirection != Vector2.zero) isMoving = true; 

        anim.SetMove(isMoving);
        if (isMoving)
        {
            anim.SetDirection(moveDirection);
        }
        else return;

        
        thisRigidbody2D.position += moveDirection * playerSpeed * Time.fixedDeltaTime;
    }
}
