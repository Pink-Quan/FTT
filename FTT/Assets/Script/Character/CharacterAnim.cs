using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnim : MonoBehaviour
{
    [HideInInspector] public Vector2 moveDirection=Vector2.zero;
    [HideInInspector] public bool isMoving =false;
    private Animator animator; 
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        SetDirection(new Vector2(0, -1));  
    }

    public void SetDirection(Vector2 moveDirection)
    {
        this.moveDirection = moveDirection;
        animator.SetFloat("HorizontalMoverment", this.moveDirection.x);
        animator.SetFloat("VerticalMoverment", this.moveDirection.y);
    }  
    
    public void SetMove(bool isMoving)
    {
        this.isMoving = isMoving;
        animator.SetBool("IsMoving", this.isMoving);
    }    
}
