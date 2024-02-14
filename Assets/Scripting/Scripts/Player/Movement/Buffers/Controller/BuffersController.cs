using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffersController : MonoBehaviour
{
    [SerializeField] Buffer JumpBuffer;
    [SerializeField] Buffer CoyoteTime;


    Coroutine BufferReference;

    void Start()
    {
        
    }

    public void SetJumpBuffer()
    {
        if (!JumpBuffer.ActivityInfo.SecondValue)
            return;
        if (BufferReference is null)
            BufferReference = StartCoroutine(JumpBuffer.StartTimerRoutine());
        else
            JumpBuffer.RenewTimer();
    }

    void Update()
    {
        
    }
}
