using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeDetection : MonoBehaviour
{
    [Header("Ray Values")]
    [SerializeField] Vector3 RayPosition;
    [SerializeField] Vector3 RayOffset;
    [SerializeField] float RayDistance;

    [Header("Slope Variables")]
    [SerializeField] LayerMask LeftShortLayer;
    [SerializeField] LayerMask RightShortLayer;
    [SerializeField] LayerMask LeftLongLayer;
    [SerializeField] LayerMask RightLongLayer;
    bool isLeftShort;
    bool isRightShort;
    bool isLeftLong;
    bool isRightLong;

    [Header("Output")]
    [SerializeField] MovementStateVariable ActualState;
    [SerializeField] PlayerVarsSO playerVariables;
    [SerializeField] IntVariable X;
    [SerializeField] FloatVariable SlopeModifier;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ActualState.Value is not GroundMovementState)
            return;
        Raycast();
        SlopeModifier.Value = ChooseModifier();
    }

    void Raycast()
    {
        isLeftLong = Physics2D.Raycast(transform.position + RayPosition, Vector3.down, RayDistance, LeftLongLayer) ||
            Physics2D.Raycast(transform.position + RayPosition + RayOffset, Vector3.down, RayDistance, LeftLongLayer) ||
            Physics2D.Raycast(transform.position + RayPosition - RayOffset, Vector3.down, RayDistance, LeftLongLayer);
        
        isRightLong = Physics2D.Raycast(transform.position + RayPosition, Vector3.down, RayDistance, RightLongLayer) ||
            Physics2D.Raycast(transform.position + RayPosition + RayOffset, Vector3.down, RayDistance, RightLongLayer) ||
            Physics2D.Raycast(transform.position + RayPosition - RayOffset, Vector3.down, RayDistance, RightLongLayer);

        isLeftShort = Physics2D.Raycast(transform.position + RayPosition, Vector3.down, RayDistance, LeftShortLayer) ||
            Physics2D.Raycast(transform.position + RayPosition + RayOffset, Vector3.down, RayDistance, LeftShortLayer) ||
            Physics2D.Raycast(transform.position + RayPosition - RayOffset, Vector3.down, RayDistance, LeftShortLayer);

        isRightShort = Physics2D.Raycast(transform.position + RayPosition, Vector3.down, RayDistance, RightShortLayer) ||
            Physics2D.Raycast(transform.position + RayPosition + RayOffset, Vector3.down, RayDistance, RightShortLayer) ||
            Physics2D.Raycast(transform.position + RayPosition - RayOffset, Vector3.down, RayDistance, RightShortLayer);
    }

    float ChooseModifier()
    {
        if (isLeftLong)
        {
            if (X.Value >= 0)
                return playerVariables.goingUpLong;
            else
                return playerVariables.goingDownLong;
        }
        if (isRightLong)
        {
            if (X.Value >= 0)
                return playerVariables.goingDownLong;
            else
                return playerVariables.goingUpLong;
        }
        if (isLeftShort)
        {
            if (X.Value >= 0)
                return playerVariables.goingUpShort;
            else
                return playerVariables.goingDownShort;
        }
        if (isRightShort)
        {
            if (X.Value >= 0)
                return playerVariables.goingDownShort;
            else
                return playerVariables.goingUpShort;
        }
        return 1f;
    }
}
