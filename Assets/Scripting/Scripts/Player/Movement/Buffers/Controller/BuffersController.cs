using UnityEngine;

public class BuffersController : MonoBehaviour
{
    [SerializeField] Buffer JumpBuffer;
    [SerializeField] Buffer CoyoteTime;


    Coroutine JumpBufferCoroutine;
    Coroutine CoyoteTimeCoroutine;

    void Start()
    {
        OnStateChanged();
    }

    public void OnStateChanged() // <- Call od State machiny
    {
        JumpBuffer.OnStateChangeCheck();
        CoyoteTime.OnStateChangeCheck();
    }

    #region Jump Buffer

    public void SetJumpBuffer()
    {
        if (!JumpBuffer.ActivityInfo.SecondValue)
            return;

        if (JumpBufferCoroutine is null)
            JumpBufferCoroutine = StartCoroutine(JumpBuffer.StartTimerRoutine());
        else
            JumpBuffer.RenewTimer();
    }

    #endregion

    #region CoyoteTime

    public void SetCoyoteTime()
    {
        if(!CoyoteTime.ActivityInfo.SecondValue)
            return;

        if (CoyoteTimeCoroutine is null)
            CoyoteTimeCoroutine = StartCoroutine(CoyoteTime.StartTimerRoutine());
        else
            CoyoteTime.RenewTimer();
    }
    #endregion
}
