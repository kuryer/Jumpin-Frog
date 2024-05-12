using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] TransformRuntimeValue referenceSubject;
    [Range(0.01f,1f)]
    [SerializeField] float parallaxValue;
    [Range(0.01f, 1f)]
    [SerializeField] float smoothTime;
    Vector3 startPosition;
    Vector3 velocity;
    Vector3 travel => Vector2.right * (referenceSubject.Item.position - startPosition);


    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        //transform.position = startPosition + travel * parallaxValue;
        //smoothdamp na y tylko
        transform.position = Vector3.SmoothDamp(transform.position, startPosition + travel * parallaxValue, ref velocity,smoothTime);
    }
}
