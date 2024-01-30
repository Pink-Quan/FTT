using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collider2DEvents : MonoBehaviour
{
    public UnityEvent<Collision2D, Collider2D> onCollisionEnter2D;
    public UnityEvent<Collision2D, Collider2D> onCollisionStay2D;
    public UnityEvent<Collision2D, Collider2D> onCollisionExit2D;

    public UnityEvent<Collider2D, Collider2D> onTriggerEnter2D;
    public UnityEvent<Collider2D, Collider2D> onTriggerStay2D;
    public UnityEvent<Collider2D, Collider2D> onTriggerExit2D;

    private Collider2D col;

    public Collider2D Collider2D=>col;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onCollisionEnter2D?.Invoke(collision, col);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        onCollisionStay2D?.Invoke(collision, col);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onCollisionExit2D?.Invoke(collision, col);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerEnter2D?.Invoke(collision, col);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        onTriggerStay2D?.Invoke(collision, col);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onTriggerExit2D?.Invoke(collision, col);
    }
}
