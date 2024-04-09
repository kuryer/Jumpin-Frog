using UnityEngine;

public class CameraBoundsGizmos : MonoBehaviour
{
    [SerializeField] BoxCollider2D Collider;
    [SerializeField] Color Color;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color;
        Gizmos.DrawWireCube(transform.position + (Vector3)Collider.offset, Collider.size * transform.localScale);
    }
}
