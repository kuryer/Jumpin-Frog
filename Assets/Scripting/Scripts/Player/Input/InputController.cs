using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [Header("Horizontal Input")]
    [SerializeField] FloatVariable X;

    [Header("Throw Direction")]
    [SerializeField] Vector2Variable ThrowDirection;
    [SerializeField] PlayerMovementVariables playerVariables;
    [SerializeField] List<StickInputRange> InputRanges;
    public void OnMove(InputAction.CallbackContext context)
    {
        float x = context.ReadValue<Vector2>().x;
        if (x == 0)
        {
            X.Value = 0;
            return;
        }
        X.Value = Mathf.Sign(x);
    }

    public void GetThrowDirection(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        if (direction == Vector2.zero)
            return;
        
        direction = direction.normalized;

        foreach (StickInputRange range in InputRanges)
        {
            if (range.IsInRange(direction))
            {
                ThrowDirection.Value = range.GetDirection();
                break;
            }
        }
    }
}
