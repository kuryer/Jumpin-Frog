using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpikes : MonoBehaviour
{
    public GameObject destinationPointPrefab;
    public float movingSpeed = 2f;
    [SerializeField] bool isPlatform;
    SurfaceEffector2D effector;
    SpikesDestinationPoint currentPoint;
    //SpikesDestinationPoint nextPoint;
    int currentIndex = 0;
    bool isGoingBack = false;
    public Vector3 moveTowardsPosition;
    public enum LoopModes
    {
        Around,
        BackToBack
    }
    [SerializeField] public LoopModes loopMode = LoopModes.Around;
    [HideInInspector] public List<SpikesDestinationPoint> points;

    #region Setup

    void Awake()
    {
        GetFirstPoint();
        if (isPlatform) effector = GetComponent<SurfaceEffector2D>();
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
                effector.speed *= -1;
            }
            else
            {
                currentPoint = points[currentIndex];
                currentIndex++;
                effector.speed *= -1;
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
        if(transform.position != currentPoint.transform.position)
        {
            //rb.MovePosition(currentPoint.GetPosition() * Time.deltaTime);
            float speed = movingSpeed * Time.deltaTime;
            moveTowardsPosition = Vector3.MoveTowards(transform.position, currentPoint.transform.position, speed);
            moveTowardsPosition -= transform.position;
            transform.position += moveTowardsPosition;
            return;
        }
        else
        {
            GetNextPoint();
        }
    }

    #endregion

    #region Editor Things

    public void DeletePoint(int index)
    {
        if(points[index] != null)
        {
            SpikesDestinationPoint point = points[index];
            DestroyImmediate(point.gameObject, true);
        }
    }

    #endregion
}
