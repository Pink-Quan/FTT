using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Breath : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite leftArrowSprte;
    [SerializeField] private Sprite rightArrowSprte;
    [SerializeField] private ParticleSystem effect;
    public Button breathButton;
    private InputAction inputs;

    void Start()
    {
        inputs = GameManager.instance.input.Player.Move;
    }

    private void OnDestroy()
    {
        inputs.started -= BreathChect;
    }

    private void InitBreath(Action<int> OnComplete)
    {
        this.OnComplete = OnComplete;
        spriteRenderer.enabled = true;
        HandleVariable();
        inputs.started += BreathChect;
    }

    public void StartBreath(Action<int> OnComplete, float breathTime)
    {
        InitBreath(OnComplete);
        score = 0;
        Invoke("DoneBreath", breathTime);
    }

    public void StartBreath(Action<int> OnComplete, int breathCount)
    {
        OnComplete += index =>
        {
            inputs.started -= BreathChect;
            spriteRenderer.enabled = false;
        };
        InitBreath(OnComplete);
        this.breathCount = breathCount;
    }

    int answer;
    int score;
    int breathCount;

    Action<int> OnComplete;
    private void BreathChect(InputAction.CallbackContext ctx)
    {
        Vector2 input = inputs.ReadValue<Vector2>();
        int playerAns = -1;

        if (input.x > 0)
            playerAns = 1;
        else if (input.x < 0)
            playerAns = 0;

        if (playerAns == answer)
        {
            effect.Play();
            score++;
        }
        else
        {
            score--;
            player.anim.TakeDamge();
        }
        HandleVariable();

        breathCount--;
        if (breathCount == 0) OnComplete?.Invoke(score);
    }

    private void HandleVariable()
    {
        answer = Random.Range(0, 2);
        if (answer == 0)
        {
            spriteRenderer.sprite = leftArrowSprte;
        }
        else if (answer == 1)
        {
            spriteRenderer.sprite = rightArrowSprte;
        }
    }

    private void DoneBreath()
    {
        inputs.started -= BreathChect;
        spriteRenderer.enabled = false;
        OnComplete?.Invoke(score);
    }
}
