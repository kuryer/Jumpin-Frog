using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] TransformRuntimeValue referenceSubject;
    [SerializeField] float addedHeight;
    [Range(0.01f,1f)]
    [SerializeField] float parallaxValueY;
    [Range(0.01f, 1f)]
    [SerializeField] float parallaxValueX;

    [Range(0.01f, 1f)]
    [SerializeField] float smoothTimeY;
    [Range(0.01f, 1f)]
    [SerializeField] float smoothTimeX;
    Vector3 startPosition;
    Vector3 velocity;
    Vector3 travel => /*Vector2.right **/ (referenceSubject.Item.position - startPosition) + new Vector3(0, addedHeight, 0);


    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        Vector3 YPosition = new Vector3(0,transform.position.y, 0);
        Vector3 YDifference = new Vector3(0, startPosition.y + travel.y * parallaxValueY); 
        float YDamp = Vector3.SmoothDamp(YPosition, YDifference, ref velocity, smoothTimeY).y;


        Vector3 XPosition = new Vector3(transform.position.x, 0,0);
        Vector3 XDifference = new Vector3(startPosition.x + travel.x * parallaxValueX, 0,0);
        float XDamp = Vector3.SmoothDamp(XPosition, XDifference, ref velocity, smoothTimeX).x;
        transform.position = new Vector3(XDamp, YDamp, transform.position.z);
    }
}
