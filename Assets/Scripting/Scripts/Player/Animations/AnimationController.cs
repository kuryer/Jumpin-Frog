using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour
{
    [Header("Sprite Flip")]
    [SerializeField] SpriteRenderer SpriteRenderer;
    [SerializeField] MovementStateVariable ActualState;

    [Header("Animation Management")]
    [SerializeField] Animator animator;
    [SerializeField] AnimationStateVariable ActualAnimation;
    [SerializeField] AnimationState InitAnimationState;
    void Start()
    {
        animator.Play(InitAnimationState.AnimationName);
    }

    public void SpriteFlipCheck(InputAction.CallbackContext context)
    {
        float x = context.ReadValue<Vector2>().x;
        if (x == 0 || !context.performed || ActualState.Value is DeadState || ActualState.Value is SwingMovementState)
            return;
        if((SpriteRenderer.flipX && x > 0) || (!SpriteRenderer.flipX && x < 0))
            SpriteRenderer.flipX = !SpriteRenderer.flipX;
    }

    public void SetSpriteFlip(bool newFlipX)
    {
        SpriteRenderer.flipX = newFlipX;
    }

    public void ChangeAnimation(AnimationState animation)
    {
        if (animation == ActualAnimation.Value)
            return;
        ActualAnimation.Value = animation;
        animator.Play(ActualAnimation.Value.AnimationName);
    }
}