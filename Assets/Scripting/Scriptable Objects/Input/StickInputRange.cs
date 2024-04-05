using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Input/Stick Input Range")]
public class StickInputRange : ScriptableObject
{
    [SerializeField] Vector2 leftRange;
    [SerializeField] Vector2 rightRange;
    [SerializeField] Vector2 direction;

    public bool IsInRange(Vector2 toCheck)
    {
        bool xValid = toCheck.x > leftRange.x && toCheck.x < rightRange.x;
        bool yValid = toCheck.y > leftRange.y && toCheck.y < rightRange.y;
        return xValid && yValid;
    }

    public Vector2 GetDirection()
    {
        return direction;
    }
}
