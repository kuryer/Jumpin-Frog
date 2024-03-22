using UnityEngine;

public class PlayerReferenceCollector : MonoBehaviour
{
    [Header("Runtime Values")]
    [SerializeField] Rigidbody2DRuntimeValue Rigidbody2DValue;
    [SerializeField] BubbleActionRuntimeValue BubbleActionValue;
    [SerializeField] JumpPadControllerRuntimeValue JumpPadControllerValue;
    [SerializeField] AnimationControllerRuntimeValue AnimationControllerValue;

    [Header("References")]
    [SerializeField] Rigidbody2D Rigidbody2D;
    [SerializeField] BubbleAction BubbleAction;
    [SerializeField] JumpPadJump JumpPadController;
    [SerializeField] AnimationController AnimationController;

    [Header("To null")]
    [Tooltip("This thing is here only because it needs to be set to null by something to not crush in ground Movement State")]
    [SerializeField] Rigidbody2DRuntimeValue platformRBValue;
    private void OnEnable()
    {
        SetReferences(true);
        platformRBValue.NullItem();
    }

    void SetReferences(bool isActive)
    {
        if (isActive)
        {
            Rigidbody2DValue.SetItem(Rigidbody2D);
            BubbleActionValue.SetItem(BubbleAction);
            JumpPadControllerValue.SetItem(JumpPadController);
            AnimationControllerValue.SetItem(AnimationController);
        }
        else
        {
            Rigidbody2DValue.NullItem();
            BubbleActionValue.NullItem();
            JumpPadControllerValue.NullItem();
            AnimationControllerValue.NullItem();
        }
    }

    private void OnDisable()
    {
        SetReferences(false);    
    }
}