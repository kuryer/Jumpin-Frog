using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingPlatform : MovingTile
{
    [Header("Platform Detection")]
    bool isStandingOnPlatform;
    [SerializeField] Vector3 rayPosition;
    [SerializeField] float onDetectionRayLength;
    [SerializeField] float interiorRayLength;
    [SerializeField] LayerMask player;

    [Header("Platform Movement")]
    [Range(1f, 4f)]
    public float movingSpeed;
    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] bool worksOnDetection; // znaczy ¿e dzia³a tylko jak sie na nim stoi
    [SerializeField] LayerMask destinationPointLayer;
    bool Back2Back_isMovingBackwards;
    int currentPointNumber;
    int lastPointNumber;
    DestinationPoint currentPoint;
    [HideInInspector] public Vector3 moveTowardsPosition;
    delegate void MovementType();
    MovementType SetNextPoint;

    enum LoopModes
    {
        Around,
        BackToBack
    }
    [SerializeField] LoopModes loopMode = LoopModes.Around;

    [Header("Editor")]
    public GameObject destinationPointPrefab;


    #region Setup


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Setup();
    }

    private void OnEnable()
    {
        SubscribeOnPlayerDeath();
    }

    private void OnDisable()
    {
        UnsubscribeOnPlayerDeath();
    }

    void SubscribeOnPlayerDeath()
    {
        Helpers.PlayerHealth.OnPlayerDeath.AddListener(OnDetection_SetNextPoint);
    }

    void UnsubscribeOnPlayerDeath()
    {
        Helpers.PlayerHealth.OnPlayerDeath.RemoveListener(OnDetection_SetNextPoint);

    }

    private void Setup()
    {
        lastPointNumber = destinationPoints.Count - 1;
        if (!worksOnDetection)
        {
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
        if(currentPointNumber == lastPointNumber)
        {
            StopMoving();
            return;
        }
        currentPointNumber++;
        currentPoint = destinationPoints[currentPointNumber];
        SetMoveTowardsPos();
    }

    void OnDetection_SetNextPoint_Backwards()
    {
        if (currentPointNumber == 0)
        {
            StopMoving();
            return;
        }
        currentPointNumber--;
        currentPoint = destinationPoints[currentPointNumber];
        SetMoveTowardsPos();
    }

    void OnDetection_SetNextPoint()
    {
        if (isStandingOnPlatform)
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

        if (!isStandingOnPlatform && hit.collider != null)
        {
            Helpers.PlayerMovement.SetPlatformTransform(rb, true);
            isStandingOnPlatform = true;
            if (worksOnDetection)
            {
                SetNextPoint();
            }
        }
        else if (isStandingOnPlatform && hit.collider == null)
        {
            Helpers.PlayerMovement.SetPlatformTransform(rb, false);
            isStandingOnPlatform = false;
        }
    }

    void CastInteriorRay()
    {
        Vector2 position = new Vector2(transform.position.x - (interiorRayLength / 2), transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(position, transform.right, interiorRayLength, destinationPointLayer);

        if (hit.collider != null && hit.collider.GetComponent<DestinationPoint>() == currentPoint)
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
    }

    #endregion


    #region Editor Things
    /*
    public void AddPoint()
    {
        DestinationPoint point = Instantiate(destinationPointPrefab, transform.parent).GetComponent<DestinationPoint>();
        destinationPoints.Add(point);
    }

    public void DeleteLastPoint()
    {
        GameObject toDelete = destinationPoints[destinationPoints.Count - 1].gameObject;
        Debug.Log(toDelete.name);
        if (toDelete != null)
        {
            destinationPoints.Remove(toDelete.GetComponent<DestinationPoint>());
            DestroyImmediate(toDelete, true );
        }
    }

    public void DeletePoint(int index)
    {
        if (destinationPoints[index] != null)
        {
            DestinationPoint point = destinationPoints[index];
            DestroyImmediate(point.gameObject, true);
        }
    }
    */
    #endregion
}
