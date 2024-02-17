using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicJumpController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerVarsSO playerVariables;

    [Header("Jump")]
    [SerializeField] Buffer JumpBuffer;
    [SerializeField] BoolVariable isGrounded;
    [SerializeField] BuffersController BuffersController;

    [Header("Anti-Double Jump")]
    [SerializeField] BoolVariable PlayerJumped;
    [SerializeField] GroundDetection GroundDetection;

    void Update()
    {
        if (PlayerJumped.Value)
            return;
        BasicJumpCheck();
    }

    void BasicJumpCheck()
    {
        if (JumpBuffer.ActivityInfo.Value() && isGrounded.Value) 
            BasicJump();
    }

    public void BasicJump()
    {
        rb.AddForce(Vector2.up * playerVariables.jumpPower, ForceMode2D.Impulse);
        //anti-double jump mechanic
        GroundDetection.PlayerJumpedCall();
        //Reset buffers
        BuffersController.ResetJumpBuffer();
        //Change state to in air
    }
}