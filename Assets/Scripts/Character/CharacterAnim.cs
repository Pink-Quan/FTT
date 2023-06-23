using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Animator))]
public class CharacterAnim : MonoBehaviour
{
    public Sprite[] charaterSprites;

    public int animIndex = 3;
    public Vector2 moveDirection = new Vector2(0, -1);
    public bool isMoving = false;
    public bool isPriorityX;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float morbundTime = 2;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        SetDirection(moveDirection);
        StartCoroutine(UpdateSprite());
    }

    public void SetDirection(Vector2 moveDirection)
    {
        if (Mathf.Abs(moveDirection.x) == Mathf.Abs(moveDirection.y))
        {
            if (isPriorityX) moveDirection.y = 0;
            else moveDirection.x = 0;
        }
        else if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            isPriorityX = true;
        }
        else if (Mathf.Abs(moveDirection.x) < Mathf.Abs(moveDirection.y))
        {
            isPriorityX = false;
        }
        this.moveDirection = moveDirection;
        animator.SetFloat("HorizontalMoverment", this.moveDirection.x);
        animator.SetFloat("VerticalMoverment", this.moveDirection.y);
    }

    public void SetMove(bool isMoving)
    {
        this.isMoving = isMoving;
        animator.SetBool("IsMoving", this.isMoving);
    }

    public void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieAnim());
    }
    public float GetMorburnTime => morbundTime;
    IEnumerator DieAnim()
    {
        float clk = 0;
        while (clk < morbundTime)
        {
            spriteRenderer.color = Color.red;
            clk += 0.5f;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.25f);

        }

        spriteRenderer.color = Color.red;
        transform.DORotate(new Vector3(0, 0, 90), 0.5f).OnComplete(() => enabled = false);
    }

    public void TakeDamge()
    {
        StartCoroutine(TakeDamgeAnim());
    }

    IEnumerator TakeDamgeAnim()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

    private IEnumerator UpdateSprite()
    {
        while (true)
        {
            yield return null;
            spriteRenderer.sprite = charaterSprites[animIndex];
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(CharacterAnim))]
public class CharaterAnimCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var t = (CharacterAnim)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Set Dirrection"))
            t.SetDirection(t.moveDirection);
        if (GUILayout.Button("Set Move"))
            t.SetMove(t.isMoving);
    }
}
#endif
