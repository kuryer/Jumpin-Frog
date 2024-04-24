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
    [SerializeField] LayerMask SlopeLayer;

    [Header("Output")]
    [SerializeField] MovementStateVariable ActualState;
    [SerializeField] BoolVariable isOnSlopeVariable;
    [SerializeField] PlayerVarsSO playerVariables;
    [SerializeField] FloatVariable X;

    // Update is called once per frame
    void FixedUpdate()
    {
        isOnSlopeVariable.Value = Raycast();
    }

    bool Raycast()
    {
        return Physics2D.Raycast(transform.position + RayPosition, Vector3.down, RayDistance, SlopeLayer) ||
            Physics2D.Raycast(transform.position + RayPosition + RayOffset, Vector3.down, RayDistance, SlopeLayer) ||
            Physics2D.Raycast(transform.position + RayPosition - RayOffset, Vector3.down, RayDistance, SlopeLayer);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + RayPosition, Vector3.down * RayDistance);
        Gizmos.DrawRay(transform.position + RayPosition - RayOffset, Vector3.down * RayDistance);
        Gizmos.DrawRay(transform.position + RayPosition + RayOffset, Vector3.down * RayDistance);
    }

    private void OnEnable()
    {
        isOnSlopeVariable.Value = Raycast();
    }

    private void OnDisable()
    {
        isOnSlopeVariable.Value = false;
    }
}
