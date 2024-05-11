using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotation : MonoBehaviour
{
    [SerializeField] TransformRuntimeValue playerTransform;
    

    void Update()
    {
        Vector3 lookPos = transform.position - playerTransform.Item.position;
        transform.Rotate(new Vector3(0, 0, lookPos.y).normalized * Time.deltaTime);    
    }
}