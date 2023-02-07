using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnim : MonoBehaviour
{
    [Tooltip("Change in inspecter to set OnEnable direction")] public Vector2 moveDirection=new Vector2(0,-1);
    [HideInInspector] public bool isMoving =false;
    private Animator animator; 
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        SetDirection(moveDirection);
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
