using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyTest : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerVarsSO playerVariables;
    PlayerControls playerControls;
    float X;
    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        X = GatherInput();
        if (Input.GetKeyDown(KeyCode.R))
            ChangeBody();
    }

    void ChangeBody()
    {
        if (rb.bodyType == RigidbodyType2D.Kinematic)
            rb.bodyType = RigidbodyType2D.Dynamic;
        else
            rb.bodyType = RigidbodyType2D.Kinematic;
    }

    float GatherInput()
    {
        float input = playerControls.Movement.Move.ReadValue<Vector2>().x;
        if (input == 0) return 0;
        else return Mathf.Sign(input);
    }

    void Movement()
    {
        //calcualte the direction we want to move in and our desired velocity
        float maxSpeed = X * playerVariables.moveSpeed;

        //calculate difference between current velocity and desired velocity
        float baseVelocity = rb.velocity.x;


        float speedDif = maxSpeed - baseVelocity;

        //change acceleration rate depending on situation
        float accelRate = (Mathf.Abs(maxSpeed) > 0.01f) ? playerVariables.acceleration : playerVariables.decceleration;

        //applies acceleration to speed difference, the raises to a set power so acceleration increases with higher speeds
        //finally multiplies by sign to reapply direction
        //float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, playerVars.velPower) * Mathf.Sign(speedDif);
        float movement = (Mathf.Abs(speedDif) * accelRate) * Mathf.Sign(speedDif);

        //anti-clipping calculations
        rb.velocity = new Vector2(rb.velocity.x + movement, rb.velocity.y);
    }
}
