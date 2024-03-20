using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Camera/Camera Variables")]
public class CameraVariables : ScriptableObject
{
    [Header("Basic Lookahead")]
    public float maxGroundLookahead;
    [Range(1,4)]
    public int smoothingValue;
    public float lookaheadAcc;
    public float lookaheadDecc;
    [Tooltip("this one is used when players lookahead is bigger than the current maximum lookahead value")]
    public float lookaheadOverspeedDecc;
    public float lookaheadStopDecc;
    [Tooltip("if velocity is less than this value lookahead will stay at 0")]
    public float minVelThreshold;
    public float minPosThreshold;
}
