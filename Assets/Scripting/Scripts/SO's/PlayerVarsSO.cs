using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Player Variables", menuName = "Scriptable Objects/ Player Variables")]
public class PlayerVarsSO : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed;
    public float acceleration;
    public float decceleration;
    public float stoppingBuffor;
    public float velPower;
    public float generalGravity;
    public float slopeSpeed;
    [Header("Slope Multipliers")]
    public float goingDownShort;
    public float goingUpShort;
    public float goingDownLong;
    public float goingUpLong;
    [Header("Platform Movement")]
    public float naturalizerModifier;
    [Header("In Air Movement")]
    public float inAirAccelerationBasic;
    public float inAirDeccelerationBasic;
    public float fallClampSpeed;
    [Header("Jump")]
    public float jumpPower;
    public float jumpCutMultiplier;
    public float fallGravity;
    public float coyoteTime;
    public float jumpBuffer;
    public float jumpTightenerTime;
    public float jumpThightenerGravity;
    [Header("Wall Jump")]
    public float wallSlideSpeed;
    public float wallGravity;
    public float wallJumpDirectionBalance;
    public float wallJumpPowerModifier;
    public float wallGrabJumpBuffer;
    public float wallGrabBuffer;
    [Header("Swing Jump")]
    public float swingJumpForce;
    public float swingJumpBalance;
    public float swingCoyoteTime;
    [Header("Swinging")]
    public float springFrequency;
    public float springDamping;
    public float swingVelocityCut;
    public float swingMoveSpeed;
    public float accelerationSwing;
    public float conccelerationSwing;
    public float deccelerationSwing;
    public float swingMovementPower;
    public float inactiveGravity;
    public float swingingGravity = 0f;
    public float swingBuffer;
    public float afterSwingJumpPower;
    public float swingSpeedPercentage;
    public float exitBarHeight;
    [Header("Bubble")]
    public float bubbleGravity;
    public float slingGravityChangeTime;
    public float throwTimer;
    public float bubbleThrowForce;
    public float throwDirectionXModifier;
    public float throwDirectionYModifier;
    [Header("Line Renderer")]
    public float lineWidth;
    [Header("Death")]
    public float respawnMoveTowardSpeed;
}
