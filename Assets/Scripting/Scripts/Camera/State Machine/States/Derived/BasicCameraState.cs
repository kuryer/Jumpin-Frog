using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Camera/Camera States/Basic")]
public class BasicCameraState : CameraState
{
    [Header("Tracking")]
    [SerializeField] TransformRuntimeValue trackedTransform;
    [SerializeField] TransformRuntimeValue trackerTransform;
    [SerializeField] Rigidbody2DRuntimeValue trackedRb;
    [SerializeField] CameraVariables cameraVariables;

    [Header("Lookahead")]
    [SerializeField] PlayerMovementVariables playerVariables;
    [SerializeField] float lookaheadValue;
    float rangeValue;
    [SerializeField] FloatVariable X;

    //new Lookahead
    [SerializeField] float lookaheadPosPercentage;
    [SerializeField] float maxGroundedPosPercentage;
    [SerializeField] float lookaheadPosition;
    [SerializeField] float lookaheadRangePos;
    public override void OnEnter()
    {
        lookaheadValue = 0;
    }
    public override void OnUpdate()
    {
        CalculateLookahead();
        ApplyCalulations();
    }

    public override void OnExit()
    {
    }
    #region Lookahead

    void ApplyCalulations()
    {
        Vector3 lookaheadVector = new Vector3(lookaheadValue, 0, 0);
        trackerTransform.Item.position = trackedTransform.Item.position + lookaheadVector;
    }


    void CalculateLookahead()
    {
        if (ZeroVelocityCheck())
            return;

        float velSign = trackedRb.Item.velocity.x == 0 ? 0f : Mathf.Sign(trackedRb.Item.velocity.x);
        float lookaheadSign = Mathf.Sign(rangeValue);
        float addedLookahead;

        if (X.Value == 0)
            addedLookahead = cameraVariables.lookaheadStopDecc;
        else
            addedLookahead = lookaheadSign == velSign ? cameraVariables.lookaheadAcc : cameraVariables.lookaheadDecc;

        addedLookahead *= Time.deltaTime;
        float velDiff = Mathf.Abs(trackedRb.Item.velocity.x) - Mathf.Abs(rangeValue);
        if (velDiff < addedLookahead && lookaheadSign == velSign)
            return;

        addedLookahead *= Mathf.Sign(trackedRb.Item.velocity.x - rangeValue);
        rangeValue += addedLookahead;

        float range = Mathf.Abs(rangeValue / RangeLimit());
        lookaheadValue = cameraVariables.maxGroundLookahead * SmoothPosition(range) * velSign;
    }

    float RangeLimit()
    {
        if (Mathf.Abs(trackedRb.Item.velocity.x) < cameraVariables.minVelThreshold)
            return cameraVariables.minVelThreshold;
        return trackedRb.Item.velocity.x;
    }

    void SmoothedCalulation()
    {
        float rangePosition = Mathf.Abs(trackedRb.Item.velocity.x) / playerVariables.maxSpeed;
        float lookaheadPosition = cameraVariables.maxGroundLookahead * SmoothPosition(rangePosition);

        if (lookaheadPosition < cameraVariables.minVelThreshold)
            lookaheadPosition = 0;

        lookaheadPosition *= Mathf.Sign(trackedRb.Item.velocity.x);
        lookaheadValue = lookaheadPosition;
    }

    void Smoothing()
    {
        float range = Mathf.Abs(lookaheadValue) / Mathf.Abs(trackedRb.Item.velocity.x);
    }

    float SmoothPosition(float rangePosition)
    {
        float result = rangePosition;
        for (int i = 1; i < cameraVariables.smoothingValue; i++)
            result *= rangePosition;
        return result;
    }
    bool ZeroVelocityCheck()
    {
        if (Mathf.Abs(trackedRb.Item.velocity.x) < cameraVariables.minVelThreshold)
            if (Mathf.Abs(lookaheadValue) < cameraVariables.minVelThreshold)
            {
                lookaheadValue = 0;
                return true;
            }
        return false;
    }

    #endregion

    #region New Lookahead



    void CalculateLookaheadPercentage()
    {
        if (lookaheadPosPercentage > maxGroundedPosPercentage /* || X.Value == 0*/)
            if (IsInVelocityDeadzone() || X.Value == 0 || SameVelAndPosDirection())
                Decrease();
            else
                Increase();
        else
            EaseInMaxVel();

        lookaheadRangePos = lookaheadPosPercentage / maxGroundedPosPercentage;
    }

    void Decrease()
    {
        if (lookaheadPosPercentage == 0)
            return;

        lookaheadPosPercentage -= cameraVariables.lookaheadDecc * Time.deltaTime;

        if (IsInVelocityDeadzone() || lookaheadPosPercentage < 0)
            lookaheadPosPercentage = 0;
    }

    void Increase()
    {
        if (lookaheadPosPercentage == maxGroundedPosPercentage)
            return;

        lookaheadPosPercentage += cameraVariables.lookaheadAcc * Time.deltaTime;

        if(lookaheadPosPercentage >= maxGroundedPosPercentage)
            lookaheadPosPercentage = maxGroundedPosPercentage;
    }

    void EaseInMaxVel()
    {
        lookaheadPosPercentage -= cameraVariables.lookaheadOverspeedDecc * Time.deltaTime;
    }
    bool IsInVelocityDeadzone()
    {
        return Mathf.Abs(trackedRb.Item.velocity.x) < cameraVariables.minVelThreshold;
    }
    bool SameVelAndPosDirection()
    {
        return Mathf.Sign(trackedRb.Item.velocity.x) == Mathf.Sign(lookaheadPosition);
    }
    void CalculateLookaheadValue()
    {

    }
    #endregion
}
