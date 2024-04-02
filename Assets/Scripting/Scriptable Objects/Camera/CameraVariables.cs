using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Camera/Camera Variables")]
public class CameraVariables : ScriptableObject
{
    [Header("Basic Lookahead")]
    public float maxGroundLookahead;
    [Range(1,4)]
    public int smoothingValue;
    [Tooltip("How fast lookahead will reach its maximum value")]
    public float lookaheadAcc;
    [Tooltip("How fast lookahead will change the direction")]
    public float lookaheadDecc;
    [Tooltip("This value is applied when lookahead is decreasing because of a near wall")]
    public float lookaheadWallDecc;
    [Tooltip("How fast lookahead will come back if the value is bigger than maximum value")]
    public float lookaheadOverspeedDecc;
    [Tooltip("How fast lookahead will stop if the player stops")]
    public float lookaheadStopDecc;
    [Tooltip("if velocity is less than this value lookahead will stay at 0")]
    public float minVelThreshold;
    public float minPosThreshold;
}
