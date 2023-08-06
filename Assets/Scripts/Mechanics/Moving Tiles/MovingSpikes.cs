using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpikes : MovingTile
{
    public GameObject destinationPointPrefab;
    public float movingSpeed = 2f;
    [SerializeField] float rayLength;
    [SerializeField] LayerMask destinationPointLayer;
    DestinationPoint currentPoint;
    int currentIndex = 0;
    bool isGoingBack = false;
    public enum LoopModes
    {
        Around,
        BackToBack
    }
    [SerializeField] public LoopModes loopMode = LoopModes.Around;
    [HideInInspector] public List<DestinationPoint> points;

    #region Setup

    void Awake()
    {
        GetFirstPoint();
    }
    void GetFirstPoint()
    {
        if(points.Count <= 1)
        {
            Debug.Log("Not enough Desitnation points to build a route.");
            return;
        }
        currentPoint = points[currentIndex];
        currentIndex++;
    }

    #endregion


    #region Movement

    void Update()
    {
        MoveTowardsCurrentPoint();
    }

    void GetNextPoint()
    {
        if(loopMode == LoopModes.Around)
        {
            if(points.Count == currentIndex)
            {
                currentIndex = 0;
                currentPoint = points[currentIndex];
                currentIndex++;
            }
            else
            {
                currentPoint = points[currentIndex];
                currentIndex++;
            }

        }
        if(loopMode == LoopModes.BackToBack)
        {
            if (!isGoingBack)
            {
                if (points.Count == currentIndex)
                {
                    currentIndex--;
                    currentPoint = points[currentIndex];
                    isGoingBack = true;
                }
                else
                {
                    currentPoint = points[currentIndex];
                    currentIndex++;
                }
            }
            else
            {
                if (currentIndex == 0)
                {
                    currentIndex++;
                    currentPoint = points[currentIndex];
                    isGoingBack = false;
                }
                else
                {
                    currentIndex--;
                    currentPoint = points[currentIndex];
                }
            }
        }
    }

    void MoveTowardsCurrentPoint()
    {
        if(points.Count > 1)
        {
            SpikesMovement();
        }
        else
        {
            Debug.Log("Platform won't go - not enough points");
        }
    }

    void SpikesMovement()
    {
        if (transform.position != currentPoint.transform.position)
        {
            float speed = movingSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, currentPoint.transform.position, speed);
        }
        else
        {
            GetNextPoint();
        }
    }


    #endregion


    #region Gizmos

    private void OnDrawGizmos()
    {
        Vector3 rayAdd = new Vector3(rayLength / 2, 0f, 0f);
        Gizmos.DrawLine(transform.position - rayAdd, transform.position + rayAdd);
    }

    #endregion
}
