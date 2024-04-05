using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] CMVCamBaseRuntimeValue basicCamera;
    [SerializeField] CinemachineVirtualCamera staticCamera;
    [SerializeField] TransformRuntimeValue playerTransform;
    [SerializeField] bool activatesFromLeftSide;

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
        if (!inDetection && canChange)
            ThresholdChange();
    }

    void ThresholdChange()
    {
        Debug.Log(IsOnStaticSide());
        if (IsOnStaticSide())
        {
            staticCamera.enabled = true;
            basicCamera.Item.enabled = false;
        }
        else
        {
            basicCamera.Item.enabled = true;
            staticCamera.enabled = false;
        }
        canChange = false;
    }

    bool IsOnStaticSide()
    {
        bool isOnRightSide = transform.position.x < playerTransform.Item.position.x;
        if (isOnRightSide)
            return activatesFromLeftSide;
        else
            return !activatesFromLeftSide;
    }

    private void FixedUpdate()
    {
        RayFunction();
    }

    void RayFunction()
    {
        inDetection = Physics2D.Raycast(transform.position, Vector2.up, rayDistance, playerLayer);
        if (inDetection && !canChange)
            canChange = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.up * rayDistance));
    }
}
