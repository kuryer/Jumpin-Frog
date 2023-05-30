using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpikes : MonoBehaviour
{
    public GameObject destinationPointPrefab;
    [SerializeField] float movingSpeed = 2f;
    [SerializeField] SpikesDestinationPoint currentPoint;
    //SpikesDestinationPoint nextPoint;
    int currentIndex = 0;
    bool isGoingBack = false;
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
        Debug.Log("current Index = " + currentIndex);
    }

    void MoveTowardsCurrentPoint()
    {
        if(transform.position != currentPoint.transform.position)
        {
            float speed = movingSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, currentPoint.transform.position, speed);

            return;
        }
        else
        {
            Debug.Log("hello");
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
