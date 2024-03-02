using UnityEngine;

public class CameraBoundsGizmos : MonoBehaviour
{
    [SerializeField] BoxCollider2D Collider;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3)Collider.offset, Collider.size * transform.localScale);
    }
}
