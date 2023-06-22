using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CharacterController : MonoBehaviour
{
    public CharacterAnim anim;
    public Animator animator;
    public Collider2D col;

    protected virtual void Start()
    {
        lastPos = transform.position;
    }


    Vector3 lastPos;
    public IEnumerator UpdateMove()
    {
        while (true)
        {
            Vector2 dir = transform.position - lastPos;
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                dir.y = 0;
                if (dir.x != 0) dir.x /= Mathf.Abs(dir.x);
            }
            else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y))
            {
                dir.x = 0;
                if (dir.y != 0) dir.y /= Mathf.Abs(dir.y);
            }
            anim.SetDirection(dir);
            lastPos = transform.position;
            anim.SetMove(true);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    public virtual void Die()
    {
        anim.Die();
    }
}
