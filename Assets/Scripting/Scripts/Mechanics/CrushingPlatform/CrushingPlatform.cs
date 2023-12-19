using UnityEngine;

public class CrushingPlatform : MonoBehaviour
{
    enum States
    {
        Idle,
        Crush,
        Respawn
    }
    States state;

    public delegate void OnCrushMethod();
    public OnCrushMethod OnCrush;

    [SerializeField] float timeToRespawn;
    [SerializeField] float rayDistance;
    [SerializeField] Vector3 rayPosition;
    [SerializeField] LayerMask playerLayer;

    void Start()
    {
        foreach(Transform child in transform)
        {
            var script = child.GetComponent<CrushingBlock>();
            script.SetTimeToRespawn(timeToRespawn);
            OnCrush += script.Crush;
        }
    }
    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + rayPosition, Vector3.right, rayDistance, playerLayer);
        if (hit.collider != null && state == States.Idle)
        {
            OnCrush();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = state == States.Idle ? Color.green : Color.red;
        Vector3 rayDistanceV3 = new Vector3(rayDistance, 0, 0);
        Gizmos.DrawLine(transform.position + rayPosition, transform.position + rayPosition + rayDistanceV3);
    }
}
