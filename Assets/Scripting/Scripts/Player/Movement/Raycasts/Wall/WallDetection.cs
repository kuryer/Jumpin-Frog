using UnityEngine;

public class WallDetection : MonoBehaviour
{
    [Header("Ray Values")]
    [SerializeField] Vector3 RayPosition;
    [SerializeField] Vector3 RayOffset;
    [SerializeField] float RayDistance;
    [SerializeField] LayerMask WallLayer;

    [Header("Detection Values")]
    [SerializeField] CombinedBool DetectedWalls;
    //First = Left || Second = Right
    void FixedUpdate()
    {
        DetectedWalls.FirstValue = Physics2D.Raycast(transform.position + RayPosition + RayOffset, Vector3.left, RayDistance, WallLayer) ||
            Physics2D.Raycast(transform.position + RayPosition + (2 * RayOffset), Vector3.left, RayDistance, WallLayer) ||
            Physics2D.Raycast(transform.position + RayPosition - (2 * RayOffset), Vector3.left, RayDistance, WallLayer) ||
            Physics2D.Raycast(transform.position + RayPosition - RayOffset, Vector3.left, RayDistance, WallLayer);
        DetectedWalls.SecondValue = Physics2D.Raycast(transform.position + RayPosition + RayOffset, Vector3.right, RayDistance, WallLayer) ||
            Physics2D.Raycast(transform.position + RayPosition + (2 * RayOffset), Vector3.right, RayDistance, WallLayer) ||
            Physics2D.Raycast(transform.position + RayPosition - (2 * RayOffset), Vector3.right, RayDistance, WallLayer) ||
            Physics2D.Raycast(transform.position + RayPosition - RayOffset, Vector3.right, RayDistance, WallLayer);
    }
}
