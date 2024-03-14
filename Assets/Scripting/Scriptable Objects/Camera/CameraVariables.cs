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
    public float lookaheadStopDecc;
    [Tooltip("if velocity is less than this value lookahead will stay at 0")]
    public float minVelThreshold;
}
