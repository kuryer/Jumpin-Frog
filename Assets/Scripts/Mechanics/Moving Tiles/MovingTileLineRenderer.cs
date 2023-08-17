using System;
using System.Collections.Generic;
using UnityEngine;


public class MovingTileLineRenderer : MonoBehaviour
{

    MovingTile movingTile;
    LineRenderer lineRenderer;
    List<GameObject> destinationPoints;

    void Update()
    {
        if (movingTile == null) movingTile = GetComponent<MovingTile>();
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        if (destinationPoints == null) destinationPoints = movingTile.destinationPoints;

        //Debug.Log(movingTile.destinationPoints.Count);
        //if (destinationPoints.Count < 2)
        //    return;


        if (movingTile.loopMode == MovingTile.LoopModes.Around)
        {
            Around_RenderLines();
            return;
        }
        Back2Back_RenderLines();

    }
    void Around_RenderLines()
    {
        lineRenderer.positionCount = destinationPoints.Count + 1;
        for (int i = 0; i <= destinationPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, GetPointPos(i));
        }
    }
    void Back2Back_RenderLines()
    {
        lineRenderer.positionCount = destinationPoints.Count;
        for (int i = 0; i < destinationPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, GetPointPos(i));
        }
    }

    Vector3 GetPointPos(int arrayIndex)
    {
        while (arrayIndex >= destinationPoints.Count)
            arrayIndex -= destinationPoints.Count;

        if (arrayIndex >= 0)
            return destinationPoints[arrayIndex].transform.position;

        throw new ArgumentException("Array index reached negative number");
    }
}
