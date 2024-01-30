using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

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
    
    public SpriteRenderer SpriteRenderer=>spriteRenderer;
    public UnityEvent onDoneDie;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        if (isStopAnimation)
            return;
        SetDirection(moveDirection);
        StartCoroutine(UpdateSprite());
    }

    public void SetDirection(Vector2 moveDirection)
    {
        if (Mathf.Abs(moveDirection.x) == Mathf.Abs(moveDirection.y))
        {
            if (isPriorityX)
            {
                moveDirection.y = 0;
                if (moveDirection.x != 0) moveDirection.x /= Mathf.Abs(moveDirection.x);
            }
            else
            {
                moveDirection.x = 0;
                if (moveDirection.y != 0) moveDirection.y /= Mathf.Abs(moveDirection.y);
            }
        }
        else if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            isPriorityX = true;
            moveDirection.y = 0;
            if (moveDirection.x != 0) moveDirection.x /= Mathf.Abs(moveDirection.x);
        }
        else if (Mathf.Abs(moveDirection.x) < Mathf.Abs(moveDirection.y))
        {
            isPriorityX = false;
            moveDirection.x = 0;
            if (moveDirection.y != 0) moveDirection.y /= Mathf.Abs(moveDirection.y);
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
        transform.DORotate(new Vector3(0, 0, 90), 0.5f).OnComplete(() => onDoneDie?.Invoke());
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

    public void ResetAnim()
    {
        spriteRenderer.sprite = charaterSprites[3];
        spriteRenderer.color = Color.white;
        transform.rotation = Quaternion.identity;
        OnEnable();
    }
    public void RandomDelayAnim()
    {
        StopAllCoroutines();
        StartCoroutine(Delay(Random.Range(0.1f, 1f), ResetAnim));
    }

    public IEnumerator Delay(float time, System.Action onDone)
    {
        yield return new WaitForSeconds(time);
        onDone?.Invoke();
    }

    public void PlayAnimation()
    {
        SetDirection(moveDirection);
        StartCoroutine(UpdateSprite());
    }
    bool isStopAnimation;
    public void StopAnimation()
    {
        isStopAnimation = true;
        spriteRenderer.sprite = charaterSprites[3];
        spriteRenderer.color = Color.white;
        StopAllCoroutines();
    }
    public float DieTime => morbundTime + 0.5f;
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
