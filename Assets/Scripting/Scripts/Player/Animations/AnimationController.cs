using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour
{
    [Header("Sprite Flip")]
    [SerializeField] SpriteRenderer SpriteRenderer;
    [SerializeField] MovementStateVariable ActualState;
    float flipDirection;
    [Header("Animation Management")]
    [SerializeField] Animator animator;
    [SerializeField] AnimationStateVariable ActualAnimation;
    [SerializeField] AnimationState InitAnimationState;
    [SerializeField] BoolVariable isInRespawnAnimation;
    void Start()
    {
        animator.Play(InitAnimationState.AnimationName);
        ActualAnimation.Value = InitAnimationState;
    }

    private void Update()
    {
        SpriteFlipCheck();
    }

    public void SpriteFlipCall(InputAction.CallbackContext context)
    {
        flipDirection = context.ReadValue<Vector2>().x;
    }

    public void SpriteFlipCheck()
    {
        if (flipDirection == 0 || ActualState.Value is SwingMovementState)
            return;
        if ((SpriteRenderer.flipX && flipDirection > 0) || (!SpriteRenderer.flipX && flipDirection < 0))
            SpriteRenderer.flipX = !SpriteRenderer.flipX;
    }

    public void SpriteFlipCheck(float flipDir)
    {
        flipDirection = flipDir;
        SpriteFlipCheck();
    }

    public void SetSpriteFlip(bool newFlipX)
    {
        SpriteRenderer.flipX = newFlipX;
    }

    public void ChangeAnimation(AnimationState animation)
    {
        if (animation == ActualAnimation.Value || isInRespawnAnimation.Value)
            return;
        ActualAnimation.Value = animation;
        animator.Play(ActualAnimation.Value.AnimationName);
    }

    private void OnDisable()
    {
        isInRespawnAnimation.Value = false;
    }
}