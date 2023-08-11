using System;
using System.Collections.Generic;
using UnityEngine;

public class MovingTile : MonoBehaviour
{
    [HideInInspector] public delegate void MovementType();
    public enum LoopModes
    {
        Around,
        BackToBack
    }

    [Header("Data")]
    public List<GameObject> destinationPoints;
    public GameObject destinationPointPrefab;
    public LoopModes loopMode = LoopModes.Around;
    public LayerMask destinationPointLayer;

    [Header("Line Renderer")]
    LineRenderer lineRenderer;


    private void Awake()
    {
        Setup();
    }
    void Setup()
    {
        if (destinationPoints.Count < 2)
            return;

        if (loopMode == MovingTile.LoopModes.Around)
        {
            Around_RenderLines();
            return;
        }
        Back2Back_RenderLines();
    }


    #region Line Renderer

    void Around_RenderLines()
    {
        for (int i = 0; i <= destinationPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, GetPointPos(i));
        }
    }

    void Back2Back_RenderLines()
    {
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

    #endregion


    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        if (destinationPoints == null || destinationPoints.Count < 2)
            return;

        if (loopMode == MovingTile.LoopModes.Around)
        {
            Around_GizmosBuild();
            return;
        }
        Back2Back_GizmosBuild();
    }

    void Around_GizmosBuild()
    {
        for (int i = 0; i <= destinationPoints.Count; i++)
        {
            Gizmos.DrawLine(GetPointPos(i), GetPointPos(i + 1)); ;
        }
    }

    void Back2Back_GizmosBuild()
    {
        for (int i = 0; i < destinationPoints.Count; i++)
        {
            Gizmos.DrawLine(GetPointPos(i), GetPointPos(i + 1)); ;
        }
    }

    #endregion

}
