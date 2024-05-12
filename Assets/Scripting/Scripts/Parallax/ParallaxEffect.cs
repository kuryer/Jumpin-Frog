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
        // nah stillo potrzebny damping na X ale oddzielny
        Vector3 YPosition = new Vector3(0,transform.position.y, 0);
        Vector3 YDifference = new Vector3(0, startPosition.y + travel.y * parallaxValue); 
        float YDamp = Vector3.SmoothDamp(YPosition, YDifference, ref velocity, smoothTime).y;
        float XPos = startPosition.x + travel.x * parallaxValue;
        transform.position = new Vector3(XPos, YDamp, transform.position.z);
    }
}
