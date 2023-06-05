using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpikesDestinationPoint : MonoBehaviour
{
    public SpikesDestinationPoint nextPoint;

    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        SetupLine();
    }

    void SetupLine()
    {
        if(nextPoint != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, nextPoint.transform.position);
        }
    }

    public Vector2 GetPosition()
    {
        Vector2 pos =  new Vector2 (transform.position.x, transform.position.y);
        return pos;
    }
}
