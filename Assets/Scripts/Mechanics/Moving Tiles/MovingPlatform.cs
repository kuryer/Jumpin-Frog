using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Detection")]
    MovingPlatform script;
    bool isCalled;
    [SerializeField] Vector3 rayPosition;
    [SerializeField] float rayLength;
    [SerializeField] LayerMask player;

    [Header("Platform Movement")]
    public GameObject destinationPointPrefab;
    public float movingSpeed = 170f;
    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] bool worksOnDetection;
    public delegate void MovementType();
    MovementType movement;
    [SerializeField] float radiusLength;
    [SerializeField] LayerMask destinationPointLayer;
    [SerializeField] SpikesDestinationPoint firstPoint;
    [SerializeField] SpikesDestinationPoint lastPoint;
    SpikesDestinationPoint currentPoint;
    int currentIndex = 0;
    bool isGoingBack = false;
    [HideInInspector] public Vector3 moveTowardsPosition;
    public enum LoopModes
    {
        Around,
        BackToBack
    }
    [SerializeField] public LoopModes loopMode = LoopModes.Around;
    [HideInInspector] public List<SpikesDestinationPoint> points;

    #region Setup

    private void Awake()
    {
        script = GetComponent<MovingPlatform>();
        rb = GetComponent<Rigidbody2D>();
        GetFirstPoint();
        if (worksOnDetection) StopMoving();
        else SetMoveTowardsPos();
    }
    void GetFirstPoint()
    {
        if (points.Count <= 1)
        {
            Debug.Log("Not enough Desitnation points to build a route.");
            return;
        }
        if (!worksOnDetection)
        {
            currentIndex++;
            currentPoint = points[currentIndex];
        }
    }
    void SetMoveTowardsPos()
    {
        moveTowardsPosition = currentPoint.transform.position - transform.position;
    }
    #endregion


    #region Update
    private void FixedUpdate()
    {
        CastRay();
        CastCircle();
        BasicPlatformMovement();
    }

    #endregion


    #region Movement

    void BasicPlatformMovement()
    {
        float speed = movingSpeed * Time.deltaTime;
        rb.velocity = moveTowardsPosition.normalized * speed;
    }
    void GetNextPoint()
    {
        if (!worksOnDetection)
        {
            if (loopMode == LoopModes.Around)
            {
                BasicAroundPoint();
            }
            if (loopMode == LoopModes.BackToBack)
            {
                BasicBackToBackPoint();
            }
            SetMoveTowardsPos();
        }
        else
        {
            if (isCalled)
            {
                OnDetectionForwardPoint();
            }
            else
            {
                OnDetectionBackwardsPoint();
            }
        }
    }

    void BasicAroundPoint()
    {
        if (points.Count == currentIndex)
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

    void BasicBackToBackPoint()
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

    void OnDetectionForwardPoint()
    {
        if (currentPoint == lastPoint)
        {
            StopMoving();
        }
        else
        {
            currentIndex = points.IndexOf(currentPoint);
            ChangeIndex(1);
            currentPoint = points[currentIndex];
            SetMoveTowardsPos();
        }
    }

    void OnDetectionBackwardsPoint()
    {
        if(currentPoint == firstPoint)
        {
            StopMoving();
        }
        else
        {
            currentIndex = points.IndexOf(currentPoint);
            ChangeIndex(-1);
            currentPoint = points[currentIndex];
            SetMoveTowardsPos();
        }
    }

    void StopMoving()
    {
        moveTowardsPosition = Vector3.zero;
    }

    void ChangeIndex(int amount)
    {
        currentIndex += amount;

        while(currentIndex >= points.Count)
        {
            int realIndex = currentIndex - points.Count;
            currentIndex = realIndex;
        }
        while(currentIndex < 0)
        {
            int realIndex = currentIndex;
            currentIndex = points.Count + realIndex;
        }
    }
    #endregion

    #region Raycast Detection

    void CastRay()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + rayPosition, Vector3.right, rayLength, player);

        if (!isCalled && hit.collider != null)
        {
            Helpers.PlayerMovement.SetPlatformTransform(script, true);
            isCalled = true;
            if (worksOnDetection)
            {
                GetNextPoint();
                SetMoveTowardsPos();
            }
        }
        else if (isCalled && hit.collider == null)
        {
            Helpers.PlayerMovement.SetPlatformTransform(script, false);
            isCalled = false;
        }
    }

    void CastCircle()
    {
        Vector2 position = new Vector2(transform.position.x - (radiusLength / 2), transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(position, transform.right, radiusLength, destinationPointLayer);

        //RaycastHit2D hit = Physics2D.CircleCast(transform.position, radiusLength, Vector2.right);
        if (hit.collider != null && hit.collider.GetComponent<SpikesDestinationPoint>() == currentPoint)
        {
            GetNextPoint();
        }
    }

    #endregion


    #region Gizmos


    private void OnDrawGizmos()
    {
        Vector3 addLength = new Vector3(rayLength, 0f, 0f);
        Vector3 addRadius = new Vector3(radiusLength / 2, 0f, 0f);
        Gizmos.DrawLine(transform.position + rayPosition, transform.position + rayPosition + addLength);
        Gizmos.DrawLine(transform.position - addRadius, transform.position + addRadius);
    }

    #endregion


    #region Editor Things

    public void DeletePoint(int index)
    {
        if (points[index] != null)
        {
            SpikesDestinationPoint point = points[index];
            DestroyImmediate(point.gameObject, true);
        }
    }

    #endregion
}
