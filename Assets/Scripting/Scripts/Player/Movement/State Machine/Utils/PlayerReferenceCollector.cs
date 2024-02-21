using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferenceCollector : MonoBehaviour
{
    [Header("Runtime Values")]
    [SerializeField] Rigidbody2DRuntimeValue Rigidbody2DValue;

    [Header("References")]
    [SerializeField] Rigidbody2D Rigidbody2D;

    private void Awake()
    {
        SetReferences();
    }

    void SetReferences()
    {
        Rigidbody2DValue.SetItem(Rigidbody2D);

    }
}