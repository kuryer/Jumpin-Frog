using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTile : MonoBehaviour
{
    [Header("Destination Points")][SerializeField]
    GameObject destPointPrefab;
    public List<DestinationPoint> destinationPoints;
    protected delegate void MovementType();
    public enum LoopModes
    {
        Around,
        BackToBack
    }
    public LoopModes loopMode = LoopModes.Around;

    public void AddPoint()
    {
        DestinationPoint point = Instantiate(destPointPrefab, transform.parent).GetComponent<DestinationPoint>();
        destinationPoints.Add(point);
    }

    public void DeleteLastPoint()
    {
        GameObject toDelete = destinationPoints[destinationPoints.Count - 1].gameObject;
        Debug.Log(toDelete.name);
        if (toDelete != null)
        {
            destinationPoints.Remove(toDelete.GetComponent<DestinationPoint>());
            DestroyImmediate(toDelete, true);
        }
    }
}
