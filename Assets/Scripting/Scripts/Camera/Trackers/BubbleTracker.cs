using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTracker : MonoBehaviour
{
    [Header("Tracking")]
    [SerializeField] TransformRuntimeValue playerTransform;

    [Header("Lookahead")]
    [SerializeField] Vector2Variable ThrowVariable;
    [SerializeField] float lookaheadValue;

    void Update()
    {
        Vector3 Lookahead = ThrowVariable.Value * lookaheadValue;
        transform.localPosition = playerTransform.Item.position - transform.parent.position + Lookahead;
    }
}
