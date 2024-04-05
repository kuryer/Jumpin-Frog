using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DefaultTracker : MonoBehaviour
{
    [Header("Tracking")]
    [SerializeField] TransformRuntimeValue playerTransform;
    [SerializeField] Rigidbody2DRuntimeValue playerRb;
    [SerializeField] CameraVariables cameraVariables;

    [Header("Lookahead")]
    [SerializeField] FloatVariable X;
    [Tooltip("No need for touching, do not touch")]
    float maxValueReachDuration = 6;
    [SerializeField] float maxLookahead;
    [SerializeField] bool smoothStopFunc;
    [SerializeField] float lookaheadPosPercentage;
    float lookaheadPosValue;
    float lookaheadValue;
    float moveDirection;

    [Header("Wall Raycast")]
    [SerializeField] LayerMask validLayer;
    [SerializeField] LayerMask blockingLayer;
    [SerializeField] Vector3 rayOffset;
    [SerializeField] float raycastDistance;
    [SerializeField] bool isTouchingWall;

    [Header("Height Bar")]
    [SerializeField] float heightBarHeight;
    [SerializeField] float groundLevel;
    [SerializeField] BoolVariable isGrounded;

    [Header("Height Diff Lerp")]
    [SerializeField] float groundDifferenceDeadZone;
    [SerializeField] float transitionDuration;
    [SerializeField] bool isFirstGroundCall;
    [SerializeField] bool isInTransition;
    private void Start()
    {
        transform.localPosition = playerTransform.Item.position;
        groundLevel = transform.localPosition.y;
    }

    void Update()
    {
        CalculateHeightBarValue();
        CalculateLookaheadPercentage();
        CalculateLookaheadValue();
        ApplyCalulations();
    }

    private void FixedUpdate()
    {
        WallRaycast();
    }

    void ApplyCalulations()
    {
        Vector3 HeightBarVector = new Vector3 (0, groundLevel, 0);
        Vector3 playerPosition = playerTransform.Item.position - transform.parent.position;
        transform.localPosition = new Vector3(playerPosition.x + lookaheadValue, HeightBarVector.y, playerPosition.z);
    }

    #region Lookahead

    void CalculateLookaheadValue()
    {
        if (smoothStopFunc) SmoothStopFunction();
        else SmoothAccelerateFunction();
    }

    void SmoothStopFunction()
    {
        lookaheadValue = maxLookahead * (1 - SmoothPosition(1 - lookaheadPosPercentage)) * moveDirection;
    }

    void SmoothAccelerateFunction()
    {
        lookaheadValue = maxLookahead * SmoothPosition(lookaheadPosPercentage) * moveDirection;
    }

    float SmoothPosition(float rangePosition)
    {
        float result = rangePosition;
        for (int i = 1; i < cameraVariables.smoothingValue; i++)
            result *= rangePosition;
        return result;
    }

    void CalculateLookaheadPercentage()
    {
        if (maxValueReachDuration >= lookaheadPosValue)
        {
            if (isTouchingWall || IsInVelocityDeadzone() || X.Value == 0 || !SameVelAndPosDirection)
                Decrease();
            else
                Increase();
        }
        else
            EaseInMaxVel();

        lookaheadPosPercentage = lookaheadPosValue / maxValueReachDuration;
    }

    void Decrease()
    {
        if (lookaheadPosValue == 0)
            return;

        float decc = isTouchingWall ? cameraVariables.lookaheadWallDecc :
            X.Value == 0 ? cameraVariables.lookaheadStopDecc : cameraVariables.lookaheadDecc;
        lookaheadPosValue -= decc * Time.deltaTime;

        moveDirection = Mathf.Sign(lookaheadValue);


        if (IsInPositionDeadzone() || lookaheadPosValue < 0)
            lookaheadPosValue = 0;
    }

    void Increase()
    {
        if (lookaheadPosValue == maxValueReachDuration)
            return;

        lookaheadPosValue += cameraVariables.lookaheadAcc * Time.deltaTime;
        moveDirection = X.Value;

        if (lookaheadPosValue >= maxValueReachDuration)
            lookaheadPosValue = maxValueReachDuration;
    }

    void EaseInMaxVel()
    {
        lookaheadPosValue -= cameraVariables.lookaheadOverspeedDecc * Time.deltaTime;
    }

    bool IsInPositionDeadzone()
    {
        return Mathf.Abs(lookaheadValue) < cameraVariables.minPosThreshold && X.Value == 0;
    }

    bool IsInVelocityDeadzone()
    {
        return Mathf.Abs(playerRb.Item.velocity.x) < cameraVariables.minVelThreshold;
    }

    bool SameVelAndPosDirection
    {
        get
        {
            float lookaheadSign = lookaheadValue == 0 ? 0f : Mathf.Sign(lookaheadValue);
            if (X.Value == lookaheadSign)
                return true;
            if (X.Value != 0 && lookaheadSign == 0)
                return true;

            return false;
        }
    }

    #endregion

    #region Wall Lookahead

    void WallRaycast()
    {
        RaycastHit2D hitR = Physics2D.Raycast(playerTransform.Item.position + rayOffset, Vector2.right, raycastDistance, validLayer);
        bool castThruBlockerR = Physics2D.Raycast(playerTransform.Item.position + rayOffset, Vector2.right, raycastDistance, blockingLayer);
        RaycastHit2D hitL = Physics2D.Raycast(playerTransform.Item.position + rayOffset, Vector2.left, raycastDistance, validLayer);
        bool castThruBlockerL = Physics2D.Raycast(playerTransform.Item.position + rayOffset, Vector2.left, raycastDistance, blockingLayer);
        isTouchingWall = (hitR.rigidbody != null && !castThruBlockerR) || (hitL.rigidbody != null && !castThruBlockerL);
    }

    #endregion

    #region Height Bar

    void CalculateHeightBarValue()
    {
        if (isGrounded.Value)
            GroundCalculation();
        else
            InAirCalculation();
    }

    void GroundCalculation()
    {
        if (isFirstGroundCall)
            GroundDifferenceCheck();
        if (isInTransition)
            return;
        groundLevel = playerTransform.Item.position.y;
    }

    void GroundDifferenceCheck()
    {
        isFirstGroundCall = false;

        if (Mathf.Abs(groundLevel - playerTransform.Item.position.y) <= groundDifferenceDeadZone
            || playerTransform.Item.position.y < groundLevel)
            return;
        StartCoroutine(TransitionToNewGroundLevel());
        isInTransition = true;
    }
    [SerializeField] float elapsedTime;
    IEnumerator TransitionToNewGroundLevel()
    {
        elapsedTime = 0f;
        while (elapsedTime <= transitionDuration)
        {
            groundLevel = Mathf.Lerp(groundLevel, playerTransform.Item.position.y , elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isInTransition = false;
    }

    void InAirCalculation()
    {
        if(!isFirstGroundCall)
            isFirstGroundCall = true;

        float value = playerTransform.Item.position.y - groundLevel;
        if(value < 0)
        {
            groundLevel = playerTransform.Item.position.y;
            return;
        }
        float heightValue = value < heightBarHeight ? 0 : value - heightBarHeight;
        groundLevel += heightValue;
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        if (playerTransform.Item != null)
        {
            Gizmos.DrawLine(transform.position + rayOffset, transform.position + rayOffset + Vector3.right * raycastDistance);
            Gizmos.DrawLine(transform.position + rayOffset, transform.position + rayOffset + Vector3.left * raycastDistance);
        }
    }
}
