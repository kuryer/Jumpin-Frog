using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Camera/Camera Variables")]
public class CameraVariables : ScriptableObject
{
    [Header("Basic Lookahead")]
    public float lookaheadAcc;
    public float lookaheadDecc;
    [Tooltip("if velocity is less than this value lookahead will stay at 0")]
    public float minVelThreshold;
}
