using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    [Header("Ray Values")]
    [SerializeField] Vector3 RayPosition;
    [SerializeField] Vector3 RayOffset;
    [SerializeField] float RayDistance;
    [SerializeField] LayerMask GroundLayer;

    [Header("Event Values")]
    bool isGrounded;
    bool isEventCalled;



    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(isGrounded != IsGrounded())
        {
            //call event;
        }
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast((transform.position + RayPosition), Vector3.down, RayDistance, GroundLayer) ||
        Physics2D.Raycast((transform.position + RayPosition) - (2 * RayOffset), Vector3.down, RayDistance, GroundLayer) ||
        Physics2D.Raycast((transform.position + RayPosition) - RayOffset, Vector3.down, RayDistance, GroundLayer) ||
        Physics2D.Raycast((transform.position + RayPosition) + (2 * RayOffset), Vector3.down, RayDistance, GroundLayer) ||
        Physics2D.Raycast((transform.position + RayPosition) + RayOffset, Vector3.down, RayDistance, GroundLayer);
    }
}
