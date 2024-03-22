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
    [SerializeField] FloatVariable X;
    [SerializeField] FloatVariable lookaheadPosPercentage;
    [SerializeField] FloatVariable actualMaxPosPercentage;
    [SerializeField] FloatVariable lookaheadPosition;
    [SerializeField] float maxPosPercentage;
    [SerializeField] float maxLookahead;
    [SerializeField] float lookaheadRangePos;
    [SerializeField] bool SmoothStopFunc;
    float moveDirection;

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


    #region New Lookahead
    void CalculateLookaheadPercentage()
    {
        if (actualMaxPosPercentage.Value >= lookaheadPosPercentage.Value /* || X.Value == 0*/)
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

        float decc = X.Value == 0 ? cameraVariables.lookaheadStopDecc : cameraVariables.lookaheadDecc;
        lookaheadPosPercentage.Value -= decc * Time.deltaTime;

        moveDirection = Mathf.Sign(lookaheadPosition.Value);


        if (IsInPositionDeadzone() || lookaheadPosPercentage.Value < 0)
            lookaheadPosPercentage.Value = 0;
    }

    void Increase()
    {
        if (lookaheadPosPercentage.Value == actualMaxPosPercentage.Value)
            return;

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
        return Mathf.Abs(lookaheadPosition.Value) < cameraVariables.minPosThreshold && X.Value == 0;
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
        if(SmoothStopFunc)SmoothStopFunction();
        else SmoothAccelerateFunction();

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
