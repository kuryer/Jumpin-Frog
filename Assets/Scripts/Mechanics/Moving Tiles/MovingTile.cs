using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTile : MonoBehaviour
{
    [Header("Destination Points")]
    public LoopModes loopMode = LoopModes.Around;
    protected delegate void MovementType();
    public enum LoopModes
    {
        Around,
        BackToBack
    }
}
