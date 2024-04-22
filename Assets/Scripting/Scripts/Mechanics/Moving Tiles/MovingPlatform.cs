using System;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Detection")]

    bool isStandingOnPlatform;
    [SerializeField] LayerMask player;
    [SerializeField] Vector3 rayPosition;
    [SerializeField] float onDetectionRayLength;
    [SerializeField] float interiorRayLength;
    [SerializeField] Vector3 upperRaysOffset;
    [SerializeField] float upperRaysLength;

    [Header("Platform Movement")]
    [Tooltip("Platform activates when the player steps on it")]
    [SerializeField] bool worksOnDetection;
    [Tooltip("If true immediately starts ggoing backwards, when player jumps of the platform")]
    [SerializeField] bool isSensitive;
    bool reachedEnd;
    bool isAbovePlatform;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Vector3 moveTowardsPosition;
    [Range(1f, 4f)] [SerializeField]
    float movingSpeed;
    int lastPointNumber;
    int currentPointNumber;
    bool Back2Back_isMovingBackwards;
    GameObject currentPoint;
    MovingTile.MovementType SetNextPoint;
    [HideInInspector] List<GameObject> destinationPoints;
    LayerMask destinationPointLayer;
    MovingTile.LoopModes loopMode;

    [Header("New Controller")]
    [SerializeField] Rigidbody2DRuntimeValue platformRbValue;

    #region Setup

    private void Awake()
    {
        GetVariables();
        Setup();
    }
    private void GetVariables()
    {
        rb = GetComponent<Rigidbody2D>();
        var dataScript = GetComponent<MovingTile>();
        destinationPoints = dataScript.destinationPoints;
        destinationPointLayer = dataScript.destinationPointLayer;
        loopMode = dataScript.loopMode;
    }

    public void OnPlayerDeath()
    {
        if (worksOnDetection && !isGoingBack)
            SetNextPoint();
    }

    bool isGoingBack;

    private void Setup()
    {
        lastPointNumber = destinationPoints.Count - 1;
        if (!worksOnDetection)
        {
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
            return;
        }
        SetNextPoint = OnDetection_SetNextPoint;
    }

    #endregion


    #region Update

    private void FixedUpdate()
    {
        CastRay();
        CastInteriorRay();
    }

    #endregion


    #region LoopMode_Around

    private void Around_SetNextPoint()
    {
        currentPoint = destinationPoints[Around_GetNextPointNumber()];
        SetMoveTowardsPos();
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
        if(currentPointNumber == 0)
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
        SetMoveTowardsPos();
    }


    #endregion


    #region OnDetection

    void OnDetection_SetNextPoint_Forward()
    {
        isGoingBack = false;
        if(currentPointNumber == lastPointNumber)
        {
            StopMoving();
            reachedEnd = true;
            return;
        }
        currentPointNumber++;
        currentPoint = destinationPoints[currentPointNumber];
        SetMoveTowardsPos();
    }

    void OnDetection_SetNextPoint_Backwards()
    {
        isGoingBack = true;
        if (currentPointNumber == 0)
        {
            StopMoving();
            return;
        }
        currentPointNumber--;
        currentPoint = destinationPoints[currentPointNumber];
        reachedEnd = false;
        SetMoveTowardsPos();
    }

    void OnDetection_SetNextPoint()
    {
        setNextPointUsed = false;
        if (isStandingOnPlatform || isAbovePlatform)
            OnDetection_SetNextPoint_Forward();
        else OnDetection_SetNextPoint_Backwards();
    }

    #endregion


    #region Movement

    void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }
    void SetMoveTowardsPos()
    {
        moveTowardsPosition = currentPoint.transform.position - transform.position;
        rb.velocity = moveTowardsPosition.normalized * movingSpeed;
    }

    #endregion


    #region Raycast Detection
    void CastRay()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + rayPosition, Vector3.right, onDetectionRayLength, player);

        isAbovePlatform = Physics2D.Raycast(transform.position, Vector3.up, upperRaysLength, player) ||
            Physics2D.Raycast(transform.position + upperRaysOffset, Vector3.up, upperRaysLength, player) ||
            Physics2D.Raycast(transform.position - upperRaysOffset, Vector3.up, upperRaysLength, player) ||
            Physics2D.Raycast(transform.position + (2 * upperRaysOffset), Vector3.up, upperRaysLength, player) ||
            Physics2D.Raycast(transform.position - (2 * upperRaysOffset), Vector3.up, upperRaysLength, player) ||
            Physics2D.Raycast(transform.position + (3 * upperRaysOffset), Vector3.up, upperRaysLength, player) ||
            Physics2D.Raycast(transform.position - (3 * upperRaysOffset), Vector3.up, upperRaysLength, player);

        if (!isStandingOnPlatform && hit.collider != null)
        {
            platformRbValue.SetItem(rb);

            isStandingOnPlatform = true;
            if (worksOnDetection && !setNextPointUsed)
            {
                SetNextPoint();
                setNextPointUsed = true;
            }
        }
        else if (isStandingOnPlatform && hit.collider == null && !isAbovePlatform)
        {
            platformRbValue.NullItem();
            isStandingOnPlatform = false;
            if (isSensitive || reachedEnd)
                SetNextPoint();
        }
    }

    bool setNextPointUsed;

    void CastInteriorRay()
    {
        Vector2 position = new Vector2(transform.position.x - (interiorRayLength / 2), transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(position, transform.right, interiorRayLength, destinationPointLayer);

        if (hit.collider != null && hit.collider.gameObject == currentPoint)
        {
            SetNextPoint();
        }
    }

    #endregion


    #region Gizmos


    private void OnDrawGizmosSelected()
    {
        Vector3 addLength = new Vector3(onDetectionRayLength, 0f, 0f);
        Vector3 addRadius = new Vector3(interiorRayLength / 2, 0f, 0f);
        Gizmos.DrawLine(transform.position + rayPosition, transform.position + rayPosition + addLength);
        Gizmos.DrawLine(transform.position - addRadius, transform.position + addRadius);
        if (worksOnDetection)
        {
            Gizmos.DrawRay(transform.position, Vector3.up * upperRaysLength);
            Gizmos.DrawRay(transform.position + upperRaysOffset, Vector3.up * upperRaysLength);
            Gizmos.DrawRay(transform.position - upperRaysOffset, Vector3.up * upperRaysLength);
            Gizmos.DrawRay(transform.position + (2 * upperRaysOffset), Vector3.up * upperRaysLength);
            Gizmos.DrawRay(transform.position - (2 * upperRaysOffset), Vector3.up * upperRaysLength);
            Gizmos.DrawRay(transform.position + (3 * upperRaysOffset), Vector3.up * upperRaysLength);
            Gizmos.DrawRay(transform.position - (3 * upperRaysOffset), Vector3.up * upperRaysLength);    
        }
    }

    #endregion
}
