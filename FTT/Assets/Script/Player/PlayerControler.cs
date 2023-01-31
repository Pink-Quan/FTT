using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [SerializeField] private float playerSpeed=5f;

    private KeyCode leftButton = KeyCode.A, rightButton=KeyCode.D,upButton= KeyCode.W, downButton=KeyCode.S;
    private Vector3 moveDirection=Vector3.zero;
    private Rigidbody2D thisRigidbody2D;

    private void Awake()
    {
        thisRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
       

    }

    private void FixedUpdate()
    {
        moveDirection = Vector3.zero;
        if (Input.GetKey(leftButton)) moveDirection.x = -1;
        if (Input.GetKey(rightButton)) moveDirection.x = 1;
        if (Input.GetKey(upButton)) moveDirection.y = 1;
        if (Input.GetKey(downButton)) moveDirection.y = -1;

        thisRigidbody2D.position += (Vector2)moveDirection * playerSpeed * Time.fixedDeltaTime;
    }
}
