using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThresholdTrigger : MonoBehaviour
{
    [SerializeField] CameraThresholdEvent leftSideThresholdEvent;
    [SerializeField] CameraThresholdEvent rightSideThresholdEvent;
    [SerializeField] TransformRuntimeValue playerTransform;

    [SerializeField] LayerMask playerLayer;
    [SerializeField] float rayDistance;

    bool inDetection;
    bool canChange;

    private void Update()
    {
        ChangeThresholdCheck();
    }

    void ChangeThresholdCheck()
    {
        if(!inDetection && canChange)
            ThresholdChange();
    }

    void ThresholdChange()
    {
        if (transform.position.x < playerTransform.Item.position.x)
            rightSideThresholdEvent.Raise();
        else
            leftSideThresholdEvent.Raise();
        canChange = false;
    }

    private void FixedUpdate()
    {
        RayFunction();
    }

    void RayFunction()
    {
        inDetection = Physics2D.Raycast(transform.position, Vector2.up, rayDistance, playerLayer);
        if(inDetection && !canChange)
            canChange = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.up * rayDistance));
    }
}
