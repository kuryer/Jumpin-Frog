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
        Debug.Log(direction);

        foreach (StickInputRange range in InputRanges)
        {
            if (range.IsInRange(direction))
            {
                ThrowDirection.Value = range.GetDirection();
                break;
            }
        }


        //if(direction.y == 0 || direction.x == 0)
        //{
        //    ThrowDirection.Value = direction;
        //    return;
        //}
        //else
        //{
        //    ThrowDirection.Value = new Vector2(.71f * Mathf.Sign(direction.x), .71f * Mathf.Sign(direction.y));
        //    ThrowDirection.Value.x *= playerVariables.XThrowModifier;
        //    ThrowDirection.Value.y *= playerVariables.YThrowModifier;
        //}
    }
}
