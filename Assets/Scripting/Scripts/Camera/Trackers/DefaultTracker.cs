using System.Collections;
using System.Collections.Generic;
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

    [Header("Height Bar")]
    [SerializeField] float heightBarHeight;
    [SerializeField] float heightValue;
    [SerializeField] float groundLevel;
    [SerializeField] BoolVariable isGrounded;

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

    void ApplyCalulations()
    {
        Vector3 HeightBarVector = new Vector3 (0, groundLevel + heightValue, 0);
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
            if (IsInVelocityDeadzone() || X.Value == 0 || !SameVelAndPosDirection)
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

        float decc = X.Value == 0 ? cameraVariables.lookaheadStopDecc : cameraVariables.lookaheadDecc;
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
        groundLevel = playerTransform.Item.position.y;
        heightValue = 0;
    }

    void InAirCalculation()
    {
        float value = playerTransform.Item.position.y - groundLevel;
        if(value < 0)
        {
            groundLevel = playerTransform.Item.position.y;
            heightValue = 0;
            return;
        }
        heightValue = value < heightBarHeight ? 0 : value - heightBarHeight;
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Vector3 heightBar = new Vector3(playerTransform.Item.position.x, groundLevel + heightBarHeight, 0);
        Vector3 groundLevelV = new Vector3(playerTransform.Item.position.x, groundLevel, 0);
        Gizmos.DrawLine(groundLevelV, heightBar);
    }
}
