using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityChaseMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D rb;

    [Header("Variables")]
    [SerializeField] float initSpeed;
    [SerializeField] Vector2 Direction;
    float speed;

    void Start()
    {
        speed = initSpeed;    
    }

    void FixedUpdate()
    {
        rb.velocity = Direction.normalized * speed;    
    }
}
