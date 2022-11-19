using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EntranceScript : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera1;
    [SerializeField] CinemachineVirtualCamera virtualCamera2;

    [SerializeField] float leftRayOffset;
    [SerializeField] float rightRayOffset;

    [SerializeField] bool showColliders;
    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Switch();
        }
    }
    */
    void Switch()
    {
        Debug.Log(virtualCamera2.Priority);

        if (virtualCamera2.Priority.Equals(1))
        {
            GoFirst();
        }
        else
        {
            GoSecond();
        }

    }
    void GoFirst()
    {
        virtualCamera2.Priority = 2;
        virtualCamera1.Priority = 1;
        Debug.Log("Go First");
    }
    void GoSecond()
    {
        virtualCamera2.Priority = 1;
        virtualCamera1.Priority = 2;
        Debug.Log("Go Second");

    }

    private void FixedUpdate()
    {
        //Left Raycast
        RaycastHit2D hitL = Physics2D.Raycast(transform.position - new Vector3(leftRayOffset, 0, 0), Vector3.up, 8);
        if (hitL.collider != null && hitL.collider.CompareTag("Player") && virtualCamera2.Priority.Equals(2))
        {
            GoSecond();
        }
        //Right Raycast
        RaycastHit2D hitR = Physics2D.Raycast(transform.position + new Vector3(rightRayOffset, 0, 0), Vector3.up, 8);
        if (hitR.collider != null && hitR.collider.CompareTag("Player") && virtualCamera1.Priority.Equals(2))
        {
            GoFirst();
        }
        if (showColliders)
        {
            Debug.Log(hitL.collider);
            Debug.Log(hitR.collider);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position - new Vector3(leftRayOffset, 0, 0), transform.position - new Vector3(leftRayOffset, 0, 0) + Vector3.up * 100f);
        Gizmos.DrawLine(transform.position + new Vector3(rightRayOffset, 0, 0), transform.position + new Vector3(rightRayOffset, 0, 0) + Vector3.up * 100f);
    }

}
