using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldSlopeDetection : MonoBehaviour
{
    [SerializeField] float rayDistance;
    [SerializeField] Vector3 rayOffset;
    [SerializeField] LayerMask leftSlopeLayer;
    [SerializeField] LayerMask rightSlopeLayer;
    [SerializeField] bool isOnRightSlope;
    [SerializeField] bool isOnLeftSlope;
    void Start()
    {
        
    }

    void Update()
    {
        isOnLeftSlope = Physics2D.Raycast(transform.position + rayOffset, -transform.up, rayDistance, leftSlopeLayer)
            || Physics2D.Raycast(transform.position - rayOffset, -transform.up, rayDistance, leftSlopeLayer);
        isOnRightSlope = Physics2D.Raycast(transform.position + rayOffset, -transform.up, rayDistance, rightSlopeLayer)
            || Physics2D.Raycast(transform.position - rayOffset, -transform.up, rayDistance, rightSlopeLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + rayOffset, -transform.up * rayDistance);
        Gizmos.DrawRay(transform.position - rayOffset, -transform.up * rayDistance);
    }
}
