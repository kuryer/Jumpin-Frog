using UnityEngine;

public class MovingSpikes : MovingTile
{
    public float movingSpeed = 2f;
    DestinationPoint currentPoint;
    MovementType SetNextPoint;
    bool Back2Back_isMovingBackwards;
    int currentPointNumber;
    int lastPointNumber;

    [SerializeField] LoopModes loopMode = LoopModes.Around;



    #region Setup

    void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        lastPointNumber = destinationPoints.Count - 1;
        switch (loopMode)
        {
            case LoopModes.Around:
                SetNextPoint = Around_SetNextPoint;
                break;
            case LoopModes.BackToBack:
                SetNextPoint = Back2Back_SetNextPoint;
                break;
        }
        SetNextPoint();
    }

    #endregion


    #region Update
    void Update()
    {
        MoveTowardsCurrentPoint();
    }
    #endregion


    #region LoopMode_Around

    private void Around_SetNextPoint()
    {
        currentPoint = destinationPoints[Around_GetNextPointNumber()];
    }

    private int Around_GetNextPointNumber()
    {
        currentPointNumber++;
        if (currentPointNumber <= lastPointNumber)
            return currentPointNumber;
        currentPointNumber = 0;
        return currentPointNumber;
    }

    #endregion


    #region LoopMode_Back2Back


    private void Back2Back_SetDirection()
    {
        if (currentPointNumber == lastPointNumber)
            Back2Back_isMovingBackwards = true;
        if (currentPointNumber == 0)
            Back2Back_isMovingBackwards = false;
    }

    private int Back2Back_GetNextPointNumber()
    {
        if (!Back2Back_isMovingBackwards)
            return ++currentPointNumber;
        return --currentPointNumber;
    }

    private void Back2Back_SetNextPoint()
    {
        Back2Back_SetDirection();
        currentPoint = destinationPoints[Back2Back_GetNextPointNumber()];
    }


    #endregion


    #region Movement

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
