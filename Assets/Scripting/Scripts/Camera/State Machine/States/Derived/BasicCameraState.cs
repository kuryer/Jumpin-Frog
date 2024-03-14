using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Camera/Camera States/Basic")]
public class BasicCameraState : CameraState
{
    [Header("Tracking")]
    [SerializeField] TransformRuntimeValue trackedTransform;
    [SerializeField] TransformRuntimeValue trackerTransform;
    [SerializeField] Rigidbody2DRuntimeValue trackedRb;
    [SerializeField] CameraVariables cameraVariables;

    [Header("Lookahead")]
    [SerializeField] float lookaheadValue;
    [SerializeField] FloatVariable X;
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
        float lookaheadSign = Mathf.Sign(lookaheadValue);
        float addedLookahead = 0;

        if (X.Value == 0)
            addedLookahead = cameraVariables.lookaheadStopDecc;
        else if(velSign != 0)
            addedLookahead = lookaheadSign == velSign ? cameraVariables.lookaheadAcc : cameraVariables.lookaheadDecc;
        
        addedLookahead *= Time.deltaTime;
        float velDiff = Mathf.Abs(trackedRb.Item.velocity.x) - Mathf.Abs(lookaheadValue);
        if (velDiff < addedLookahead && lookaheadSign == velSign)
            return;

        addedLookahead *= Mathf.Sign(trackedRb.Item.velocity.x - lookaheadValue);
        lookaheadValue += addedLookahead;
    }

    bool ZeroVelocityCheck()
    {
        if(Mathf.Abs(trackedRb.Item.velocity.x) < cameraVariables.minVelThreshold)
            if(Mathf.Abs(lookaheadValue) < cameraVariables.minVelThreshold)
            {
                lookaheadValue = 0;
                return true;
            }
        return false;
    }

    #endregion
}
