using UnityEngine;

public class BubbleArrow : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Vector2Variable ThrowDirection;

    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(ThrowDirection.Value.y, ThrowDirection.Value.x) - 90);
    }

    public void Enable()
    {
        enabled = true;
        transform.localRotation.Set(0, 0, Mathf.Atan2(ThrowDirection.Value.x, ThrowDirection.Value.y), 0);
        spriteRenderer.enabled = true;
    }

    public void Disable()
    {
        enabled = false;
        spriteRenderer.enabled = false;
    }
}
