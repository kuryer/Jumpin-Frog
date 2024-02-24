using UnityEngine;

public class PlayerReferenceCollector : MonoBehaviour
{
    [Header("Runtime Values")]
    [SerializeField] Rigidbody2DRuntimeValue Rigidbody2DValue;

    [Header("References")]
    [SerializeField] Rigidbody2D Rigidbody2D;

    private void OnEnable()
    {
        SetReferences(true);
    }

    void SetReferences(bool isActive)
    {
        if (isActive)
            Rigidbody2DValue.SetItem(Rigidbody2D);
        else
            Rigidbody2DValue.NullItem();
    }

    private void OnDisable()
    {
        SetReferences(false);    
    }
}