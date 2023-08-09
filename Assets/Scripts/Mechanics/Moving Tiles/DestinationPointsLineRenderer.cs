using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DestinationPointsLineRenderer : MonoBehaviour
{
    List<DestinationPoint> points;
    LineRenderer lineRenderer;
    MovingTile.LoopModes loopMode;
    private void Awake()
    {
        GetVariables();

        Setup();
    }

    void GetVariables()
    {
        //points = GetComponent<MovingTile>().destinationPoints;
        lineRenderer = GetComponent<LineRenderer>();
        loopMode = GetComponent<MovingTile>().loopMode;
    }

    void Setup()
    {
        if (points.Count < 2)
            return;

        if(loopMode == MovingTile.LoopModes.Around)
        {
            Around_RenderLines();
            return;
        }
        Back2Back_RenderLines();
    }


    void Around_RenderLines()
    {
        for(int i = 0; i <= points.Count; i++)
        {
            lineRenderer.SetPosition(i, GetPointPos(i));
        }
    }

    void Back2Back_RenderLines()
    {
        for( int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, GetPointPos(i));
        }
    }

    Vector3 GetPointPos(int arrayIndex)
    {
        while(arrayIndex >= points.Count)
            arrayIndex -= points.Count;

        if(arrayIndex >= 0)
            return points[arrayIndex].transform.position;

        throw new ArgumentException("Array index reached negative number");
    }

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        if(points.Count < 2)
            return;

        if(loopMode == MovingTile.LoopModes.Around)
        {
            Around_GizmosBuild();
            return;
        }
        Back2Back_GizmosBuild();
    }

    void Around_GizmosBuild()
    {
        for (int i = 0; i <= points.Count; i++)
        {
            Gizmos.DrawLine(GetPointPos(i), GetPointPos(i + 1)); ;
        }
    }

    void Back2Back_GizmosBuild()
    {
        for (int i = 0; i < points.Count; i++)
        {
            Gizmos.DrawLine(GetPointPos(i), GetPointPos(i + 1)); ;
        }
    }
    #endregion

}
