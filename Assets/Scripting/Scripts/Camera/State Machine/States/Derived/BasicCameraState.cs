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
    [SerializeField] FloatVariable lookaheadPosPercentage;
    [SerializeField] FloatVariable actualMaxPosPercentage;
    [SerializeField] FloatVariable lookaheadPosition;
    [SerializeField] float maxPosPercentage;
    [SerializeField] float maxLookahead;
    [SerializeField] float lookaheadRangePos;
    float moveDirection;

    [SerializeField] bool increase;
    [SerializeField] bool decrease;
    public override void OnEnter()
    {
        if (actualMaxPosPercentage.Value == 0)
            actualMaxPosPercentage.Value = maxPosPercentage;
    }
    public override void OnUpdate()
    {
        CalculateLookaheadPercentage();
        CalculateLookaheadValue();
        ApplyCalulations();
    }

    public override void OnExit()
    {
    }

    void ApplyCalulations()
    {
        Vector3 lookaheadVector = new Vector3(lookaheadPosition.Value, 0, 0);
        trackerTransform.Item.position = trackedTransform.Item.position + lookaheadVector;
    }

    #region Lookahead

    //void CalculateLookahead()
    //{
    //    if (ZeroVelocityCheck())
    //        return;

    //    float velSign = trackedRb.Item.velocity.x == 0 ? 0f : Mathf.Sign(trackedRb.Item.velocity.x);
    //    float lookaheadSign = Mathf.Sign(rangeValue);
    //    float addedLookahead;

    //    if (X.Value == 0)
    //        addedLookahead = cameraVariables.lookaheadStopDecc;
    //    else
    //        addedLookahead = lookaheadSign == velSign ? cameraVariables.lookaheadAcc : cameraVariables.lookaheadDecc;

    //    addedLookahead *= Time.deltaTime;
    //    float velDiff = Mathf.Abs(trackedRb.Item.velocity.x) - Mathf.Abs(rangeValue);
    //    if (velDiff < addedLookahead && lookaheadSign == velSign)
    //        return;

    //    addedLookahead *= Mathf.Sign(trackedRb.Item.velocity.x - rangeValue);
    //    rangeValue += addedLookahead;

    //    float range = Mathf.Abs(rangeValue / RangeLimit());
    //    lookaheadValue = cameraVariables.maxGroundLookahead * SmoothPosition(range) * velSign;
    //}

    //float RangeLimit()
    //{
    //    if (Mathf.Abs(trackedRb.Item.velocity.x) < cameraVariables.minVelThreshold)
    //        return cameraVariables.minVelThreshold;
    //    return trackedRb.Item.velocity.x;
    //}

    //void SmoothedCalulation()
    //{
    //    float rangePosition = Mathf.Abs(trackedRb.Item.velocity.x) / playerVariables.maxSpeed;
    //    float lookaheadPosition = cameraVariables.maxGroundLookahead * SmoothPosition(rangePosition);

    //    if (lookaheadPosition < cameraVariables.minVelThreshold)
    //        lookaheadPosition = 0;

    //    lookaheadPosition *= Mathf.Sign(trackedRb.Item.velocity.x);
    //    lookaheadValue = lookaheadPosition;
    //}

    //void Smoothing()
    //{
    //    float range = Mathf.Abs(lookaheadValue) / Mathf.Abs(trackedRb.Item.velocity.x);
    //}


    //bool ZeroVelocityCheck()
    //{
    //    if (Mathf.Abs(trackedRb.Item.velocity.x) < cameraVariables.minVelThreshold)
    //        if (Mathf.Abs(lookaheadValue) < cameraVariables.minVelThreshold)
    //        {
    //            lookaheadValue = 0;
    //            return true;
    //        }
    //    return false;
    //}

    #endregion

    #region New Lookahead



    void CalculateLookaheadPercentage()
    {
        if (actualMaxPosPercentage.Value > lookaheadPosPercentage.Value /* || X.Value == 0*/)
        {
            if (IsInVelocityDeadzone() || X.Value == 0 || !SameVelAndPosDirection)
                Decrease();
            else
                Increase();
        }
        else
            EaseInMaxVel();

        lookaheadRangePos = lookaheadPosPercentage.Value / actualMaxPosPercentage.Value;
    }

    void Decrease()
    {
        if (lookaheadPosPercentage.Value == 0)
            return;

        lookaheadPosPercentage.Value -= cameraVariables.lookaheadDecc * Time.deltaTime;

        decrease = true;
        increase = false;

        moveDirection = Mathf.Sign(lookaheadPosition.Value);


        if (IsInPositionDeadzone() || lookaheadPosPercentage.Value < 0)
            lookaheadPosPercentage.Value = 0;
    }

    void Increase()
    {
        if (lookaheadPosPercentage.Value == actualMaxPosPercentage.Value)
            return;

        increase = true;
        decrease = false;

        lookaheadPosPercentage.Value += cameraVariables.lookaheadAcc * Time.deltaTime;
        moveDirection = X.Value;

        if (lookaheadPosPercentage.Value >= actualMaxPosPercentage.Value)
            lookaheadPosPercentage.Value = actualMaxPosPercentage.Value;
    }

    void EaseInMaxVel()
    {
        lookaheadPosPercentage.Value -= cameraVariables.lookaheadOverspeedDecc * Time.deltaTime;


        if (lookaheadPosPercentage.Value <= maxPosPercentage)
            actualMaxPosPercentage.Value = maxPosPercentage;
    }

    bool IsInPositionDeadzone()
    {
        return Mathf.Abs(lookaheadPosition.Value) < cameraVariables.minPosThreshold;
    }

    bool IsInVelocityDeadzone()
    {
        return Mathf.Abs(trackedRb.Item.velocity.x) < cameraVariables.minVelThreshold;
    }

    bool SameVelAndPosDirection
    {
        get
        {
            float lookaheadSign = lookaheadPosition.Value == 0 ? 0f : Mathf.Sign(lookaheadPosition.Value);
            if (X.Value == lookaheadSign)
                return true;
            if (X.Value != 0 && lookaheadSign == 0)
                return true;

            return false;
        }
    }
    void CalculateLookaheadValue()
    {
        if(X.Value == 0)
            SmoothStopFunction();
        else
            SmoothAccelerateFunction();
    }

    void SmoothStopFunction()
    {
        lookaheadPosition.Value = maxLookahead * (1 - SmoothPosition(1 - lookaheadRangePos)) * moveDirection;
    }

    void SmoothAccelerateFunction()
    {
        lookaheadPosition.Value = maxLookahead * SmoothPosition(lookaheadRangePos) * moveDirection;
    }

    float SmoothPosition(float rangePosition)
    {
        float result = rangePosition;
        for (int i = 1; i < cameraVariables.smoothingValue; i++)
            result *= rangePosition;
        return result;
    }
    #endregion
}
