using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5f;
    private float defaultSpeed;
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
        moveInput.started += OnPlayerStartMove;
    }
    private void OnDisable()
    {
        moveInput.started -= OnPlayerStartMove;
    }

    private void Start()
    {
        defaultSpeed = playerSpeed;
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

    public void SetSpeed(float speed)
    {
        playerSpeed=speed;
    }

    public float Speed { get { return playerSpeed; } }
    public float DefaultSpeed { get { return defaultSpeed; } }

    public void ResetPlayerSpeed()
    {
        playerSpeed = defaultSpeed;
    }
}
