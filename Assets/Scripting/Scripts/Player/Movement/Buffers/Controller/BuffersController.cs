using UnityEngine;

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

    public void SetJumpBuffer()
    {
        if (!JumpBuffer.ActivityInfo.SecondValue)
            return;

        if (!JumpBuffer.ActivityInfo.FirstValue)
            JumpBuffer.Coroutine = StartCoroutine(JumpBuffer.StartTimerRoutine());
        else
            JumpBuffer.RenewTimer();
    }

    public void ResetJumpBuffer()
    {
        StopCoroutine(JumpBuffer.Coroutine);
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
        StopCoroutine(CoyoteTime.Coroutine);
        CoyoteTime.ResetTimer();
    }

    #endregion

    #region Wall Jump Buffer

    public void SetWallJumpBuffer()
    {
        if (!WallJumpBuffer.ActivityInfo.SecondValue)
            return;

        if (!WallJumpBuffer.ActivityInfo.FirstValue)
            WallJumpBuffer.Coroutine = StartCoroutine(JumpBuffer.StartTimerRoutine());
        else
            WallJumpBuffer.RenewTimer();
    }

    public void ResetWallJumpBuffer()
    {
        StopCoroutine(WallJumpBuffer.Coroutine);
        WallJumpBuffer.ResetTimer();
    }
    #endregion

    #region Swing Catch Buffer

    public void SetSwingCatchBuffer()
    {
        if (!SwingCatchBuffer.ActivityInfo.SecondValue)
            return;

        if (!SwingCatchBuffer.ActivityInfo.FirstValue)
            SwingCatchBuffer.Coroutine = StartCoroutine(JumpBuffer.StartTimerRoutine());
        else
            SwingCatchBuffer.RenewTimer();
    }

    public void ResetSwingCatchBuffer()
    {
        StopCoroutine(SwingCatchBuffer.Coroutine);
        SwingCatchBuffer.ResetTimer();
    }

    #endregion

    #region Bubble Buffer

    public void SetBubbleBuffer()
    {
        if (!BubbleBuffer.ActivityInfo.SecondValue)
            return;

        if (!BubbleBuffer.ActivityInfo.FirstValue)
            BubbleBuffer.Coroutine = StartCoroutine(JumpBuffer.StartTimerRoutine());
        else
            BubbleBuffer.RenewTimer();
    }

    public void ResetBubbleBuffer()
    {
        StopCoroutine(BubbleBuffer.Coroutine);
        BubbleBuffer.ResetTimer();
    }

    #endregion

    #region Swing Jump Coyote Time

    public void SetSwingJumpCoyoteTime()
    {
        if (!SwingJumpCoyoteTime.ActivityInfo.SecondValue)
            return;

        if (!SwingJumpCoyoteTime.ActivityInfo.FirstValue)
            SwingJumpCoyoteTime.Coroutine = StartCoroutine(JumpBuffer.StartTimerRoutine());
        else
            SwingJumpCoyoteTime.RenewTimer();
    }

    public void ResetSwingJumpCoyoteTime()
    {
        StopCoroutine(SwingJumpCoyoteTime.Coroutine);
        SwingJumpCoyoteTime.ResetTimer();
    }

    #endregion
}
