using UnityEngine;
using UnityEngine.InputSystem;

public class BuffersController : MonoBehaviour
{
    [SerializeField] Buffer JumpBuffer;
    [SerializeField] Buffer CoyoteTime;
    [SerializeField] Buffer WallJumpBuffer;
    [SerializeField] Buffer SwingCatchBuffer;
    [SerializeField] Buffer BubbleBuffer;
    [SerializeField] Buffer SwingJumpCoyoteTime;

    void Start()
    {
        OnStateChanged();
    }

    public void OnStateChanged() // <- Call od State machiny
    {
        JumpBuffer.OnStateChangeCheck();
        CoyoteTime.OnStateChangeCheck();
        WallJumpBuffer.OnStateChangeCheck();
        SwingCatchBuffer.OnStateChangeCheck();
        BubbleBuffer.OnStateChangeCheck();
        SwingJumpCoyoteTime.OnStateChangeCheck();
    }

    #region Jump Buffer

    public void SetJumpBuffer(InputAction.CallbackContext context)
    {
        if (!JumpBuffer.ActivityInfo.SecondValue || !context.started)
            return;

        if (!JumpBuffer.ActivityInfo.FirstValue)
            JumpBuffer.Coroutine = StartCoroutine(JumpBuffer.StartTimerRoutine());
        else
            JumpBuffer.RenewTimer();
    }

    public void ResetJumpBuffer()
    {
        if(JumpBuffer.ActivityInfo.FirstValue) StopCoroutine(JumpBuffer.Coroutine);
        JumpBuffer.ResetTimer();
    }

    #endregion

    #region CoyoteTime

    //Called by GroundDetection.InAirCall Event
    public void SetCoyoteTime()
    {
        if(!CoyoteTime.ActivityInfo.SecondValue)
            return;

        if (!CoyoteTime.ActivityInfo.FirstValue)
            CoyoteTime.Coroutine = StartCoroutine(CoyoteTime.StartTimerRoutine());
        else
            CoyoteTime.RenewTimer();
    }

    public void ResetCoyoteTime()
    {
        if(CoyoteTime.ActivityInfo.FirstValue) StopCoroutine(CoyoteTime.Coroutine);
        CoyoteTime.ResetTimer();
    }

    #endregion

    #region Wall Jump Buffer

    public void SetWallJumpBuffer(InputAction.CallbackContext context)
    {
        if (!WallJumpBuffer.ActivityInfo.SecondValue || !context.started)
            return;

        if (!WallJumpBuffer.ActivityInfo.FirstValue)
            WallJumpBuffer.Coroutine = StartCoroutine(WallJumpBuffer.StartTimerRoutine());
        else
            WallJumpBuffer.RenewTimer();
    }

    public void ResetWallJumpBuffer()
    {
        if(WallJumpBuffer.ActivityInfo.FirstValue) StopCoroutine(WallJumpBuffer.Coroutine);
        WallJumpBuffer.ResetTimer();
    }
    #endregion

    #region Swing Catch Buffer

    public void SetSwingCatchBuffer(InputAction.CallbackContext context)
    {
        if (!SwingCatchBuffer.ActivityInfo.SecondValue || !context.performed)
            return;

        if (!SwingCatchBuffer.ActivityInfo.FirstValue)
            SwingCatchBuffer.Coroutine = StartCoroutine(SwingCatchBuffer.StartTimerRoutine());
        else
            SwingCatchBuffer.RenewTimer();
    }

    public void ResetSwingCatchBuffer()
    {
        if(SwingCatchBuffer.ActivityInfo.FirstValue) StopCoroutine(SwingCatchBuffer.Coroutine);
        SwingCatchBuffer.ResetTimer();
    }

    #endregion

    #region Bubble Buffer

    public void SetBubbleBuffer(InputAction.CallbackContext context)
    {
        if (!BubbleBuffer.ActivityInfo.SecondValue || !context.started)
            return;

        if (!BubbleBuffer.ActivityInfo.FirstValue)
            BubbleBuffer.Coroutine = StartCoroutine(BubbleBuffer.StartTimerRoutine());
        else
            BubbleBuffer.RenewTimer();
    }

    public void ResetBubbleBuffer()
    {
        if(BubbleBuffer.ActivityInfo.FirstValue) StopCoroutine(BubbleBuffer.Coroutine);
        BubbleBuffer.ResetTimer();
    }

    #endregion

    #region Swing Jump Coyote Time

    public void SetSwingJumpCoyoteTime()
    {
        if (!SwingJumpCoyoteTime.ActivityInfo.SecondValue)
            return;

        if (!SwingJumpCoyoteTime.ActivityInfo.FirstValue)
            SwingJumpCoyoteTime.Coroutine = StartCoroutine(SwingJumpCoyoteTime.StartTimerRoutine());
        else
            SwingJumpCoyoteTime.RenewTimer();
    }

    public void ResetSwingJumpCoyoteTime()
    {
        if(SwingJumpCoyoteTime.ActivityInfo.FirstValue) StopCoroutine(SwingJumpCoyoteTime.Coroutine);
        SwingJumpCoyoteTime.ResetTimer();
    }

    #endregion
}
