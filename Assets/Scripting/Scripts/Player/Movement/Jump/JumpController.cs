using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerVarsSO playerVariables;

    [Header("Jump")]
    [SerializeField] Buffer JumpBuffer;



    public void BasicJump()
    {
        // tutaj mo¿na zastosowaæ zasade o rozdzielnoœci z logiki boole'a A * C + B * C == (A + B) * C
        if (JumpBuffer.ActivityInfo.Value() /* &&  isGrounded  || CoyoteTime && JumpBuffer*/) 
        {
            rb.AddForce(Vector2.up * playerVariables.jumpPower, ForceMode2D.Impulse);

        }

    }

}
