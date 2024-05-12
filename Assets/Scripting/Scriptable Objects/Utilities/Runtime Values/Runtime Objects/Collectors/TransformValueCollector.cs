using UnityEngine;

public class TransformValueCollector : MonoBehaviour
{
    [SerializeField] TransformRuntimeValue RuntimeValue;

    private void OnEnable()
    {
        RuntimeValue.SetItem(transform);
    }

    private void OnDisable()
    {
        RuntimeValue.NullItem();
    }
}
