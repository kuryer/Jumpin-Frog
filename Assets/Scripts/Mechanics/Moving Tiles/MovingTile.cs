using UnityEngine;

public class MovingTile : MonoBehaviour
{
    [Header("Destination Points")]
    public LoopModes loopMode = LoopModes.Around;
    [SerializeField] protected LayerMask destinationPointLayer = 31;
    protected delegate void MovementType();
    public enum LoopModes
    {
        Around,
        BackToBack
    }
}
