using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5f;

    private KeyCode leftButton = KeyCode.A, rightButton = KeyCode.D, upButton = KeyCode.W, downButton = KeyCode.S;
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody2D thisRigidbody2D;
    private PlayerController playerController;
    private CharacterAnim anim;

    private void Awake()
    {
        GetComponentInit();
    }

    private void GetComponentInit()
    {
        playerController = GetComponent<PlayerController>();
        thisRigidbody2D = playerController.rb;
        anim = playerController.anim;
    }    

    private void FixedUpdate()
    {
        moveDirection = Vector3.zero;
        bool isMoving = false;
        if (Input.GetKey(leftButton))  { moveDirection.x = -1; isMoving = true; }
        if (Input.GetKey(rightButton)) { moveDirection.x = 1; isMoving = true;  }
        if (Input.GetKey(upButton))    { moveDirection.y = 1; isMoving = true;  }
        if (Input.GetKey(downButton))  { moveDirection.y = -1; isMoving = true; }

        anim.SetMove(isMoving);
        if (isMoving)
        {
            anim.SetDirection(moveDirection);
        }
        else return;

        
        thisRigidbody2D.position += (Vector2)moveDirection.normalized * playerSpeed * Time.fixedDeltaTime;
    }
}
