using System.Collections.Generic;
using UnityEngine;

public class MovingSpikes : MonoBehaviour
{
    public float movingSpeed = 2f;
    GameObject currentPoint;
    [HideInInspector] List<GameObject> destinationPoints;
    MovingTile.MovementType SetNextPoint;
    MovingTile.LoopModes loopMode;

    int lastPointNumber;
    int currentPointIndex;
    private bool Back2Back_isMovingBackwards;


    #region Setup

    private void Awake()
    {
        GetVariables();
        Setup();
    }
    private void GetVariables()
    {
        var dataScript = GetComponent<MovingTile>();
        destinationPoints = dataScript.destinationPoints;
        loopMode = dataScript.loopMode;
    }

    private void Setup()
    {
        lastPointNumber = destinationPoints.Count - 1;
        switch (loopMode)
        {
            case MovingTile.LoopModes.Around:
                SetNextPoint = Around_SetNextPoint;
                break;
            case MovingTile.LoopModes.BackToBack:
                SetNextPoint = Back2Back_SetNextPoint;
                break;
        }
        SetNextPoint();
    }

    #endregion


    #region LoopMode_Around

    private void Around_SetNextPoint()
    {
        currentPoint = destinationPoints[Around_GetNextPointNumber()];
    }

    private int Around_GetNextPointNumber()
    {
        currentPointIndex++;
        if (currentPointIndex <= lastPointNumber)
            return currentPointIndex;
        currentPointIndex = 0;
        return currentPointIndex;
    }

    #endregion


    #region LoopMode_Back2Back

    private void Back2Back_SetDirection()
    {
        if (currentPointIndex == lastPointNumber)
            Back2Back_isMovingBackwards = true;
        if (currentPointIndex == 0)
            Back2Back_isMovingBackwards = false;
    }

    private int Back2Back_GetNextPointNumber()
    {
        if (!Back2Back_isMovingBackwards)
            return ++currentPointIndex;
        return --currentPointIndex;
    }

    private void Back2Back_SetNextPoint()
    {
        Back2Back_SetDirection();
        currentPoint = destinationPoints[Back2Back_GetNextPointNumber()];
    }

    #endregion


    #region Movement

    void Update()
    {
        MoveTowardsCurrentPoint();
    }


    void MoveTowardsCurrentPoint()
    {
        if(destinationPoints.Count > 1)
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
            SetNextPoint();
        }
    }


    #endregion
}
