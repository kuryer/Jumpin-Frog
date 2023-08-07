using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationPointsLineRenderer : MonoBehaviour
{
    MovingTile baseClass;

    private void Awake()
    {
        baseClass = GetComponent<MovingTile>();
    }
}
