using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class CrushingPlatform : MonoBehaviour
{
    [SerializeField] List<CrushingBlock> blocks = new List<CrushingBlock>();
    enum States
    {
        Idle,
        Crush,
        Respawn
    }
    States state;

    public delegate void OnCrushMethod();
    public OnCrushMethod OnCrush;

    [SerializeField] float rayDistance;
    [SerializeField] Vector3 rayPosition;
    [SerializeField] LayerMask playerLayer;

    void Start()
    {
        foreach(Transform child in transform)
        {
            blocks.Add(child.GetComponent<CrushingBlock>());
        }
    }
    private void Update()
    {
            OnCrush();
    }
    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + rayPosition, Vector3.right, rayDistance, playerLayer);
        if (hit.collider != null && state == States.Idle)
        {
            OnCrush();
        }
    }
}
