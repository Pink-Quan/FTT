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
    private Transform directionTransform;
    private InputAction moveInput;

    private void Awake()
    {
        GetComponentInit();
    }

    private void OnEnable()
    {
        moveInput = GameManager.instance.input.Player.Move;
        moveInput.Enable();

        moveInput.started += OnPlayerStartMove;

    }
    private void OnDisable()
    {
        moveInput.started -= OnPlayerStartMove;

        moveInput.Disable();
    }

    private void GetComponentInit()
    {
        playerController = GetComponent<PlayerController>();
        thisRigidbody2D = playerController.rb;
        anim = playerController.anim;
        directionTransform = playerController.directionTrasform.parent;
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

        var angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg + 90;
        if (isMoving) directionTransform.rotation = Quaternion.Euler(0, 0, angle);


        anim.SetMove(isMoving);
        if (isMoving)
        {
            anim.SetDirection(moveDirection);
        }
        else return;


        thisRigidbody2D.position += moveDirection * playerSpeed * Time.fixedDeltaTime;
    }

    private void OnPlayerStartMove(InputAction.CallbackContext ctx)
    {
        Vector2 firstDir = ctx.ReadValue<Vector2>();
        if (Mathf.Abs(firstDir.x) > Mathf.Abs(firstDir.y))
        {
            anim.isPriorityX = true;
        }
        else if (Mathf.Abs(firstDir.x) < Mathf.Abs(firstDir.y))
        {
            anim.isPriorityX = false;
        }
    }
}
