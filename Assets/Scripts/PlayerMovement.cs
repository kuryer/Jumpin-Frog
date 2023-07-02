using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    private float X;
    //private bool JumpDown;
    //private bool JumpHold;
    //private bool JumpUp;
    bool canMove = false;
    private float lastJumpPressed;
    Vector3 groundRayoffset;
    Vector3 wallRayoffset;


    //[SerializeField] TextMeshProUGUI gravityStateText;
    [Header("Gravity")]
    gravityState GravityState;
    enum gravityState
    {
        Normal,
        JumpTightener,
        Falling,
        WallGrab,
        Swing,
        Sling,
        Space
    }


    [Header("Movement")]
    [SerializeField] PlayerVarsSO playerVars;
    public delegate void MovementDelegate();
    public delegate void JumpDelegate();
    MovementDelegate movement;
    JumpDelegate jump;
    PlayerControls playerControls;
    Transform platformTransform;
    [SerializeField] MovingPlatform platformScript;
    Vector2 platformPosition;
    Vector2 platformPosDelta;
    Rigidbody2D platformRB;
    [SerializeField] float platfromMovementFix;
    float lastGroundedTime;
    bool isGrabbing;
    bool jumped;
    float wallJumpBuffer;
    bool isWallPauseJumping;
    float wallGrabBufferL;
    float wallGrabBufferR;
    bool jumpCut;

    [Header("Ground Collisions")]
    [SerializeField] float groundRayLength;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float rayOffset;
    [SerializeField] Vector3 groundRayPosition;
    [SerializeField] bool isGrounded;


    [Header("Wall Collisions")]
    [SerializeField] LayerMask wallLayer;
    [SerializeField] float wallRayLength;
    [SerializeField] float wallRayOffset;
    [SerializeField] float wallRayPosition;
    bool onLeftWall;
    bool onRightWall;
    bool onAnyWall => onLeftWall || onRightWall;
    bool canWallJump => onAnyWall && !isGrounded && !isWallPauseJumping && !isSwinging;

    [Header("Corner Correction Variables")]
    [SerializeField] private float topRaycastLength;
    [SerializeField] private Vector3 edgeRaycastOffset;
    [SerializeField] private Vector3 innerRaycastOffset;
    private bool canCornerCorrect;

    [Header("Bubble")]
    [SerializeField] float bubbleMagnetismStrength;
    Vector2 bubblePosition;
    BubbleScript currentBubbleScript;
    float bubbleX;
    float bubbleY;
    Vector2 bubbleThrowDirection;


    [Header("Swing")]
    SpringJoint2D distJoint;
    [SerializeField] bool canSwing;
    [SerializeField] float playerRotationFix;
    bool isSwinging;
    SwingPoint swingScript;
    Rigidbody2D swingPointRB;
    Transform swingTransform;
    float lastSwingPressed;
    bool SwingDown;
    bool SwingUp;
    bool swingBoosterCheck;
    //bool swingJumped;
    float swingJumpBuffer;
    private IEnumerator slingTimer;


    [Header("Line Renderer")]
    LineRenderer lineRenderer;
    Vector3 swingPointPosition;


    [Header("Animations")]
    PlayerAnimations playerAnims;
    [SerializeField] GameObject JumpCircle;
    [SerializeField] float jumpCircleposition;
    public bool isFacingRight = true;

    enum AnimationState
    {
        Idle_Player,
        Run_Player,
        Jump_Player,
        Fall_Player,
        WallGrab_Player,
        Swing_Player,
        InAirRoll_Player
    }
    SpriteRenderer spriteRenderer;

    [Header("TongueRenderer")]
    [SerializeField] TongueRenderer tongueRenderer;


    [Header("Camera Blend")]
    [SerializeField] CameraFollowScript cameraFollowScript;

    #region Updates and Start

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        SubscribeForPlayerInputEvents();
    }

    private void OnDisable()
    {
        playerControls.Disable();
        UnsubscribeFromPlayerInputEvents();
    }

    void SubscribeForPlayerInputEvents()
    {
        playerControls.Movement.Jump.performed += SetJumpBuffer;
        playerControls.Movement.Jump.canceled += CallJumpCut;


        playerControls.Movement.Action.performed += SetSwingBuffer;
        playerControls.Movement.Action.canceled += StopSwingingCall;

    }

    void UnsubscribeFromPlayerInputEvents()
    {
        playerControls.Movement.Jump.performed -= SetJumpBuffer;
        playerControls.Movement.Jump.canceled -= CallJumpCut;


        playerControls.Movement.Action.performed -= SetSwingBuffer;
        playerControls.Movement.Action.canceled -= StopSwingingCall;

    }

    void Start()
    {
        movement = BasicMovement;
        jump = Jump;
        rb = GetComponent<Rigidbody2D>();
        distJoint = GetComponent<SpringJoint2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = tongueRenderer.GetComponent<LineRenderer>();
        playerAnims = GetComponent<PlayerAnimations>();
        lineRenderer.widthMultiplier = playerVars.lineWidth;
        groundRayoffset = new Vector3(rayOffset, 0f, 0f);
        wallRayoffset = new Vector3(0f, wallRayOffset, 0f);
    }
    void Update()
    {
        if(canMove)GatherInput();
        GatherBubbleInput();
        FallChecker();

        //if (isBlending) return;

        jump();

        
        if (canSwing || isSwinging) Swing();
        if (canWallJump) WallJump();
        if(!isWallPauseJumping && !isSwinging) WallGrab();
        
        //if (canSling) Sling();
        //if (isSwinging) LineRendering();
        

        Timer();
        ChangeSpring();

        DetectSideOfAnimation();

        if (Input.GetKey(KeyCode.R))
        {
            Debug.Log("movement funcion: " + movement.Method.Name.ToString());
        }
    }
    private void FixedUpdate()
    {
        CheckCollisions();


        if (isSwinging) SwingRotation();
        //AddPlatformVelocity();
        /*if (canMove)*/ movement();
        if (canCornerCorrect) CornerCorrect(rb.velocity.y);
        if (rb.velocity.y < 0f && !isGrounded && !isGrabbing && !isSwinging && GravityState != gravityState.Sling) FallClamp();
    }

    #endregion


    #region Movements
    private void BasicMovement()
    {
        //AddPlatformVelocity();

        //calcualte the direction we want to move in and our desired velocity
        float maxSpeed = X * playerVars.moveSpeed;
        //calculate difference between current velocity and desired velocity
        float speedDif = maxSpeed - rb.velocity.x;

        if(platformScript != null)
        {
            //rb.velocity += platformScript.rb.velocity;
            //rb.velocity += new Vector2(0, platformScript.rb.velocity.y);
            speedDif += platformScript.rb.velocity.x;
        }

        //change acceleration rate depending on situation
        float accelRate = (Mathf.Abs(maxSpeed) > 0.01f) ? playerVars.acceleration : playerVars.decceleration;
        


        //applies acceleration to speed difference, the raises to a set power so acceleration increases with higher speeds
        //finally multiplies by sign to reapply direction
        //float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, playerVars.velPower) * Mathf.Sign(speedDif);
        float movement = (Mathf.Abs(speedDif) * accelRate) * Mathf.Sign(speedDif);
        //anti-clipping calculations
        //movement *= (Mathf.Abs(rb.velocity.x) < .001f && X == 0) ? 0f : 1f;
        /*
        if(Mathf.Abs(movement) < 10f)
        {
            movement = 0f;
        }
        */
        rb.AddForce(movement * Vector2.right);
        //if(platformScript != null)
        //    Debug.Log("rb velocity: " + rb.velocity + ", speedDif: " + speedDif + ", movement: " + movement);
    }
    void InAirMovement()
    {
        //calcualte the direction we want to move in and our desired velocity
        float maxSpeed = X * playerVars.moveSpeed;
        //calculate difference between current velocity and desired velocity
        float speedDif = maxSpeed - rb.velocity.x;
        //change acceleration rate depending on situation
        float accelRate = (Mathf.Abs(maxSpeed) > 0.01f) ? playerVars.inAirAccelerationBasic : playerVars.inAirDeccelerationBasic;
        //checks if our player isnt stopping himself because of higher velocity than desired max speed
        float limiter = Mathf.Sign(speedDif).Equals(Mathf.Sign(maxSpeed)) ? 1f : 0f;
        //applies acceleration to speed difference, the raises to a set power so acceleration increases with higher speeds
        //finally multiplies by sign to reapply direction
        float movement = Mathf.Pow(Mathf.Abs(maxSpeed) * accelRate, playerVars.velPower) * X;

        movement *= limiter;
        rb.AddForce(movement * Vector2.right);
    }
    void SwingingMovement()
    {
        
        float maxSpeed = X * playerVars.swingMoveSpeed;

        //float speedDif = maxSpeed - (rb.velocity.magnitude * X);
        float speedDif = maxSpeed - (rb.velocity.x + platformPosDelta.x);
        
        speedDif = Mathf.Sign(speedDif) == Mathf.Sign(maxSpeed) ? speedDif : 0f;

        float accelRate = SwingAccelRate();

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, playerVars.swingMovementPower) * Mathf.Sign(speedDif);

        rb.AddForce(movement * transform.right);
        
    }
    float SwingAccelRate()
    {
        float accelRate = 0f;
        if (X.Equals(SwingSignVelocityX()))
        {
            accelRate = playerVars.accelerationSwing;
        }
        else if(X != SwingSignVelocityX())
        {
            accelRate = playerVars.conccelerationSwing;
        }
        if(X == 0f)
        {
            accelRate = playerVars.deccelerationSwing;
        }

        return accelRate;
    }
    float SwingSignVelocityX()
    {
        float correctVelocityX = rb.velocity.x;
        //Debug.Log(transform.localEulerAngles.z);
        if(transform.eulerAngles.z >= 90f && transform.eulerAngles.z <= 270f)
        {
            correctVelocityX = rb.velocity.x * -1f;
        }
        return Mathf.Sign(correctVelocityX);
    }
    void WallGrabMovement()
    {

    }
    #endregion


    #region Platform Velocity

    public void SetPlatformTransform(MovingPlatform script, bool isEnter)
    {
        if (isEnter)
        {
            platformScript = script;
            platformRB = script.GetComponent<Rigidbody2D>();
        }
        else
        {
            platformScript = null;
            platformRB = null;
            platformPosition = Vector3.zero;
        }
    }

    #endregion


    #region Jumps

    //===================
    //Normal Jump Section
    //===================

    private void Jump()
    {
        if (!jumped)
        {
            if(lastJumpPressed > 0 && isGrounded /* Jump Buffer */ || lastJumpPressed > 0 && lastGroundedTime > 0 /* Coyote Time */)
            {
                //Debug.Log("Basic Jump");
                jumped = true;
                playerAnims.ChangeAnimationState(AnimationState.Jump_Player.ToString());
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(Vector2.up * playerVars.jumpPower, ForceMode2D.Impulse);
                ZeroAllBuffers();
                JumpThightenerQueue();
            }
            movement = InAirMovement;
        }
    }
    void JumpCut()
    {
        if (!isGrounded && rb.velocity.y > 0.1f /* && !JumpHold */ && !jumpCut)
        {
            jumpCut = true;
            rb.AddForce(Vector2.down * rb.velocity.y * playerVars.jumpCutMultiplier, ForceMode2D.Impulse);
            SwitchGravity(gravityState.Falling);
        }

    }

    //=================
    //Wall Jump Section
    //=================
    
    void WallJump()
    {
        if (/*wallJumpBuffer > 0f && */lastJumpPressed > 0f)
        {
            //Debug.Log("Wall Jump");
            //isWallJumping = true;
            SwitchGravity(gravityState.Normal);
            rb.velocity = Vector2.zero;
            
            isWallPauseJumping = true;
            isGrabbing = false;
            ZeroAllBuffers();

            if (onRightWall)
            {
                rb.AddForce(((Vector2.up * playerVars.wallJumpDirectionBalance) + (Vector2.left * (1f - playerVars.wallJumpDirectionBalance))) * playerVars.jumpPower * playerVars.wallJumpPowerModifier, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(((Vector2.up * playerVars.wallJumpDirectionBalance) + (Vector2.right * (1f - playerVars.wallJumpDirectionBalance))) * playerVars.jumpPower * playerVars.wallJumpPowerModifier, ForceMode2D.Impulse);
            }


            playerAnims.ChangeAnimationState(AnimationState.InAirRoll_Player.ToString());
            jump = WaitingJump;
            movement = InAirMovement;
        }
    }

    //========================
    //After Swing Jump Section
    //========================
    void SwingJump()
    {
        if(lastJumpPressed > 0f)
        {
            //CZY JA POTRZEBUJE TERAZ TEGO BALANSU ALBO PROCENTA Z PREDKOSCI JAK MAM JUZ POGRUPOWANY X I Y NA ODDZIELNE PARTIE I DZIA£AJ¥ ONE ODDZIELNIE???
            //MOZNABY POPRÓBOWAÆ BEZ TEGO BALANSU ITD.
            //Debug.Log("SwingJump");
            //swingJumped = true;
            if(isSwinging)
                StopSwing();
            //Debug.Log(rb.velocity.y);
            SpawnJumpCircle();
            Vector2 direction = new Vector2(SwingJumpDirectionX() * X * playerVars.swingJumpBalance, SwingJumpDirectionY() * (1f - playerVars.swingJumpBalance));
            //playerAnims.ChangeAnimationState(AnimationState.Jump_Player.ToString());
            //float movePercent = MovePercentage();
            rb.velocity = Vector2.zero;
            rb.AddForce(direction * playerVars.swingJumpForce /** movePercent*/, ForceMode2D.Impulse);
            ZeroAllBuffers();
            jump = WaitingJump;
        }
    }
    float SwingJumpDirectionX()
    {
        if (Mathf.Abs(rb.velocity.x) > 2f)
            return 2f;
        else
            return Mathf.Abs(rb.velocity.x);
    }
    float SwingJumpDirectionY()
    {
        if (rb.velocity.y < 0f)
            return 0f;
        if (rb.velocity.y > 2f)
            return 2f;
        return rb.velocity.y;
    }
    void WaitingJump()
    {
        if(isGrounded && !isSwinging)
        {
            jump = Jump;
        }
    }

    #endregion


    #region Bubble

    public void StartAdjustingToTheBubbleCenter(Vector2 bubblePos, BubbleScript bubble)
    {
        bubblePosition = bubblePos;
        currentBubbleScript = bubble;
        jumpCut = true;
        SetCanMove(false);
        SwitchGravity(gravityState.Sling);
        StartCoroutine(MoveTowardsBubbleCenter());
        if(slingTimer != null) StopCoroutine(slingTimer);
    }
    
    IEnumerator MoveTowardsBubbleCenter()
    {
        float timeToPop = playerVars.throwTimer;
        playerAnims.ChangeAnimationState(AnimationState.Fall_Player.ToString());
        while (timeToPop > 0f)
        {
            rb.position = Vector2.MoveTowards(rb.position, bubblePosition, bubbleMagnetismStrength * Time.deltaTime);
            timeToPop -= Time.deltaTime;
            yield return null;
        }
        currentBubbleScript.ThrowPlayer();
    }
    public void ThrowPlayer()
    {
        //throw to the desired direction
        SetCanMove(true);
        slingTimer = SlingGravityTimer();
        StartCoroutine(slingTimer);
        rb.AddForce(ThrowDirection() * playerVars.bubbleThrowForce, ForceMode2D.Impulse);
        //Debug.Log(ThrowDirection());
        playerAnims.ChangeAnimationState(AnimationState.InAirRoll_Player.ToString());
    }
    Vector2 ThrowDirection()
    {
        //tu moge sie bawic tym vertical i horizontal dashem
        Vector2 direction;
        direction = new Vector2(bubbleX, bubbleY).normalized;
        direction.x *= playerVars.throwDirectionXModifier;
        direction.y *= playerVars.throwDirectionYModifier;
        return direction;
    }
    IEnumerator SlingGravityTimer()
    {
        SwitchGravity(gravityState.Sling);

        yield return new WaitForSeconds(playerVars.slingGravityChangeTime);
        if (GravityState.Equals(gravityState.Sling))
        {
            SwitchGravity(gravityState.Falling);
            SlowDownBubbleDash();
        }
    }
    void SlowDownBubbleDash()
    {
        rb.velocity /= 1.5f;
    }

    #endregion


    #region Swing
    void SwingRotation()
    {
        Vector2 lookDir = swingPointRB.position - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        angle -= playerRotationFix;
        rb.rotation = angle;
    }
    public void SetCanSwingTrue(Vector2 pos, Rigidbody2D rb, SwingPoint swingP)
    {
        canSwing = true;
        distJoint.connectedAnchor = pos;
        tongueRenderer.SetSwingPointPosition(new Vector3(pos.x, pos.y, 0));
        tongueRenderer.StartCalculation();
        swingScript = swingP;
        swingTransform = swingP.transform;
        swingPointPosition = pos;
        swingPointRB = rb;
    }
    public void SetCanSwingFalse()
    {
        canSwing = false;
    }
    void Swing()
    {
        if(lastSwingPressed > 0f && !isGrounded)
        {
            StartSwing();
        }
        if(/* isSwinging && SwingUp || */ isSwinging && transform.position.y >= swingTransform.position.y - playerVars.exitBarHeight && swingBoosterCheck)
        {
            StopSwing();
        }
    }
    void StartSwing()
    {
        //swingJumped = false;
        ZeroAllBuffers();
        lastSwingPressed = 0f;
        distJoint.enabled = true;
        //lineRenderer.enabled = true;
        tongueRenderer.TurnSpriteRenderer();
        SwitchGravity(gravityState.Swing);
        VelocityCut();
        playerAnims.ChangeAnimationState(AnimationState.Swing_Player.ToString());
        isSwinging = true;
        swingScript.PlaySwingAnimation();
        movement = SwingingMovement;
        jump = SwingJump;
    }
    void StopSwing()
    {
        isSwinging = false;
        distJoint.enabled = false;
        //lineRenderer.enabled = false;
        SwitchGravity(gravityState.Normal);
        swingScript.StartTimer();
        canSwing = false;
        rb.rotation = 0f;
        swingBoosterCheck = false;
        playerAnims.ChangeAnimationState(AnimationState.InAirRoll_Player.ToString());
        movement = InAirMovement;
        jump = WaitingJump;
        tongueRenderer.TurnSpriteRenderer();
        tongueRenderer.StopCalculation();
        //JumpThightenerQueue();
    }
    void VelocityCut()
    {
        //Debug.Log("Before Velocity Cut: " + rb.velocity);
        rb.velocity = new Vector2(rb.velocity.x / playerVars.swingVelocityCut, rb.velocity.y / playerVars.swingVelocityCut);
        //Debug.Log("After Velocity Cut: " + rb.velocity);
    }

    #endregion


    #region WallGrab

    void WallGrab()
    {
        if(onAnyWall && isGrabbingProperSide() && !isGrounded && !isGrabbing)
        {
            rb.velocity = Vector2.zero;
            ResetWallGrabBuffers();
            playerAnims.ChangeAnimationState(AnimationState.WallGrab_Player.ToString());
            StartCoroutine(WallGrabbing());
            //isWallJumping = false;
            movement = WallGrabMovement;
        }
    }
    IEnumerator WallGrabbing()
    {
        isGrabbing = true;
        wallJumpBuffer = playerVars.wallGrabJumpBuffer;

        StickToWall();
        //jump = WallJump;
        SwitchGravity(gravityState.WallGrab);

        while(isStillGrabbing() && !isGrounded && !isWallPauseJumping)
        {
            if (rb.velocity.y < -playerVars.wallSlideSpeed)
                rb.velocity = new Vector2(0f, -playerVars.wallSlideSpeed);
            yield return null;
        }

        SwitchGravity(gravityState.Normal);
        JumpThightenerQueue();

        isGrabbing = false;
    }
    bool isGrabbingProperSide()
    {
        if (onLeftWall && wallGrabBufferL > 0f)
            return true;
        if (onRightWall && wallGrabBufferR > 0f)
            return true;
        return false;
    }
    bool isStillGrabbing()
    {
        if (onLeftWall && X < 0f)
            return true;
        if (onRightWall && X > 0f)
            return true;
        return false;
    }
    void ResetWallGrabBuffers()
    {
        wallGrabBufferR = 0f;
        wallGrabBufferL = 0f;
    }
    void StickToWall()
    {
        if (onRightWall && X >= 0f)
        {
            rb.velocity = new Vector2(2f, 0f);
        }
        else if (onLeftWall && X <= 0f)
        {
            rb.velocity = new Vector2(-2f, 0f);
        }
    }

    #endregion


    #region Gravity


    void SwitchGravity(gravityState state)
    {
        switch (state)
        {
            case gravityState.Normal:
                {
                    rb.gravityScale = playerVars.generalGravity;
                    GravityState = gravityState.Normal;
                    //gravityStateText.text = "Gravity State: Normal";
                    break;
                }
            case gravityState.JumpTightener:
                {
                    rb.gravityScale = playerVars.jumpThightenerGravity;
                    GravityState = gravityState.JumpTightener;
                    //gravityStateText.text = "Gravity State: Jump Thightener";
                    break;
                }
            case gravityState.Falling:
                {
                    //playerAnims.ChangeAnimationState(AnimationState.Fall_Player.ToString());
                    rb.gravityScale = playerVars.fallGravity;
                    GravityState = gravityState.Falling;
                    //gravityStateText.text = "Gravity State: Falling";
                    break;
                }
            case gravityState.WallGrab:
                {
                    rb.gravityScale = playerVars.wallGravity;
                    GravityState = gravityState.WallGrab;
                    //gravityStateText.text = "Gravity State: Wall Grab";
                    break;
                }
            case gravityState.Swing:
                {
                    rb.gravityScale = playerVars.swingGravity;
                    GravityState = gravityState.Swing;
                    //gravityStateText.text = "Gravity State: Swing";
                    break;
                }
            case gravityState.Sling:
                {
                    rb.gravityScale = playerVars.bubbleGravity;
                    GravityState = gravityState.Sling;
                    //gravityStateText.text = "Gravity State: Sling";
                    break;
                }
            case gravityState.Space:
                {
                    rb.gravityScale = 0f;
                    GravityState = gravityState.Space;
                    break;
                }
        }
    }
    void FallChecker()
    {
        if (rb.velocity.y < 0 && lastGroundedTime <= 0f && !isGrabbing && !isSwinging && GravityState.Equals(gravityState.Normal) || rb.velocity.y < 0 && lastGroundedTime <= 0f && !isGrabbing && !isSwinging && GravityState.Equals(gravityState.JumpTightener))
        {
            SwitchGravity(gravityState.Falling);
        }
    }
    IEnumerator JumpThightenerTimer()
    {
        yield return new WaitForSeconds(playerVars.jumpTightenerTime);

        if(GravityState.Equals(gravityState.Normal))
            SwitchGravity(gravityState.JumpTightener);
    }
    void JumpThightenerQueue()
    {
        //StopCoroutine(JumpThightenerTimer());
        //StartCoroutine(JumpThightenerTimer());
    }
    void FallClamp()
    {
        if(playerAnims.CurrentState != AnimationState.InAirRoll_Player.ToString())
        {
            playerAnims.ChangeAnimationState(AnimationState.Fall_Player.ToString());
        }
        if (Mathf.Abs(rb.velocity.y) > playerVars.fallClampSpeed)
            rb.velocity = new Vector2(rb.velocity.x, -playerVars.fallClampSpeed);
    }
    #endregion


    #region Collision Check

    void CheckCollisions()
    {
        //Ground Collision
        isGrounded = Physics2D.Raycast((transform.position + groundRayPosition), Vector3.down, groundRayLength, groundLayer) || 
            Physics2D.Raycast((transform.position + groundRayPosition) - (2 * groundRayoffset), Vector3.down, groundRayLength, groundLayer) ||
            Physics2D.Raycast((transform.position + groundRayPosition) - groundRayoffset, Vector3.down, groundRayLength, groundLayer) ||
            Physics2D.Raycast((transform.position + groundRayPosition) + (2 * groundRayoffset), Vector3.down, groundRayLength, groundLayer) ||
            Physics2D.Raycast((transform.position + groundRayPosition) + groundRayoffset, Vector3.down, groundRayLength, groundLayer);

        //Corner Collisions
        canCornerCorrect = Physics2D.Raycast(transform.position + edgeRaycastOffset, Vector2.up, topRaycastLength, groundLayer) &&
                           !Physics2D.Raycast(transform.position + innerRaycastOffset, Vector2.up, topRaycastLength, groundLayer) ||
                           Physics2D.Raycast(transform.position - edgeRaycastOffset, Vector2.up, topRaycastLength, groundLayer) &&
                           !Physics2D.Raycast(transform.position - innerRaycastOffset, Vector2.up, topRaycastLength, groundLayer);

        //Wall Collision
        onLeftWall = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z) + wallRayoffset, Vector3.left, wallRayLength, wallLayer) ||
            Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z) + (2 * wallRayoffset), Vector3.left, wallRayLength, wallLayer) ||
            Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z) - (2 * wallRayoffset), Vector3.left, wallRayLength, wallLayer) ||
            Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z) - wallRayoffset, Vector3.left, wallRayLength, wallLayer);
        onRightWall = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z) + wallRayoffset, Vector3.right, wallRayLength, wallLayer) ||
            Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z) + (2 * wallRayoffset), Vector3.right, wallRayLength, wallLayer) ||
            Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z) - (2 * wallRayoffset), Vector3.right, wallRayLength, wallLayer) ||
            Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z) - wallRayoffset, Vector3.right, wallRayLength, wallLayer);
    }

    void CornerCorrect(float Yvelocity)
    {
        //Push player to the right
        RaycastHit2D hit = Physics2D.Raycast(transform.position - innerRaycastOffset + Vector3.up * topRaycastLength, Vector3.left, topRaycastLength, groundLayer);
        if(hit.collider != null)
        {
            float newPos = Vector3.Distance(new Vector3(hit.point.x, transform.position.y, 0f) + Vector3.up * topRaycastLength, transform.position - edgeRaycastOffset + Vector3.up * topRaycastLength);
            transform.position = new Vector3(transform.position.x + newPos, transform.position.y, transform.position.z);
            rb.velocity = new Vector2(rb.velocity.x, Yvelocity);
            return;
        }

        //Push player to the left
        hit = Physics2D.Raycast(transform.position + innerRaycastOffset + Vector3.up * topRaycastLength, Vector3.right, topRaycastLength, groundLayer);
        if(hit.collider != null)
        {
            float newPos = Vector3.Distance(new Vector3(hit.point.x, transform.position.y, 0f) + Vector3.up * topRaycastLength, transform.position + edgeRaycastOffset + Vector3.up * topRaycastLength);
            transform.position = new Vector3(transform.position.x - newPos, transform.position.y, transform.position.z);
            rb.velocity = new Vector2(rb.velocity.x, Yvelocity);
        }
    }
    private void OnDrawGizmos()
    {
        groundRayoffset = new Vector3(rayOffset, 0f, 0f);
        wallRayoffset = new Vector3(0f, wallRayOffset, 0f);
        Vector3 basePosition = new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z);
        Gizmos.color = Color.green;
        //Ground Rays
        Gizmos.DrawRay((transform.position + groundRayPosition), Vector3.down * groundRayLength);
        Gizmos.DrawRay((transform.position + groundRayPosition) - groundRayoffset, Vector3.down * groundRayLength);
        Gizmos.DrawRay((transform.position + groundRayPosition) - (2 * groundRayoffset), Vector3.down * groundRayLength);
        Gizmos.DrawRay((transform.position + groundRayPosition) + (2 * groundRayoffset), Vector3.down * groundRayLength);
        Gizmos.DrawRay((transform.position + groundRayPosition) + groundRayoffset, Vector3.down * groundRayLength);

        //Wall Rays
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z) + wallRayoffset, Vector3.right * wallRayLength);
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z) + (2 * wallRayoffset), Vector3.right * wallRayLength);
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z) - (2 * wallRayoffset), Vector3.right * wallRayLength);
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z) - wallRayoffset, Vector3.right * wallRayLength);
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z) + wallRayoffset, Vector3.left * wallRayLength);
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z) + (2 * wallRayoffset), Vector3.left * wallRayLength);
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z) - (2 * wallRayoffset), Vector3.left * wallRayLength);
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + wallRayPosition, transform.position.z) - wallRayoffset, Vector3.left * wallRayLength);

        //Corner Rays
        Gizmos.DrawLine(transform.position + edgeRaycastOffset, transform.position + edgeRaycastOffset + Vector3.up * topRaycastLength);
        Gizmos.DrawLine(transform.position - edgeRaycastOffset, transform.position - edgeRaycastOffset + Vector3.up * topRaycastLength);
        Gizmos.DrawLine(transform.position + innerRaycastOffset, transform.position + innerRaycastOffset + Vector3.up * topRaycastLength);
        Gizmos.DrawLine(transform.position - innerRaycastOffset, transform.position - innerRaycastOffset + Vector3.up * topRaycastLength);

        //Corner Distance Rays
        Gizmos.DrawLine(transform.position - innerRaycastOffset + Vector3.up * topRaycastLength, transform.position - innerRaycastOffset + Vector3.up * topRaycastLength + Vector3.left * topRaycastLength);
        Gizmos.DrawLine(transform.position + innerRaycastOffset + Vector3.up * topRaycastLength, transform.position + innerRaycastOffset + Vector3.up * topRaycastLength + Vector3.right * topRaycastLength);
    }
    #endregion


    #region Gather Input

    void SetJumpBuffer(InputAction.CallbackContext context)
    {
        lastJumpPressed = playerVars.jumpBuffer;
    }

    void CallJumpCut(InputAction.CallbackContext context)
    {
        if(jump == Jump)
            JumpCut();
    }

    void SetSwingBuffer(InputAction.CallbackContext context)
    {
        lastSwingPressed = playerVars.swingBuffer;
    }

    void StopSwingingCall(InputAction.CallbackContext context)
    {
        if(isSwinging)
            StopSwing();
    }

    

    private void GatherInput()
    {
        X = XMovement();
        GatherWallGrabBuffer();




        //JumpDown = playerControls.Movement.Jump.ReadValue<float>();
        //JumpDown = Input.GetButtonDown("Jump");
        //JumpHold = Input.GetButton("Jump");
        //JumpUp = Input.GetButtonUp("Jump");
        //SwingDown = Input.GetButtonDown("Swing");
        //SwingUp = Input.GetButtonUp("Swing");
        /*
        if (SwingDown)
            lastSwingPressed = playerVars.swingBuffer;
        */
        
    }
    float XMovement()
    {
        float input = playerControls.Movement.Move.ReadValue<Vector2>().x;
        if (input == 0) return 0;
        else return Mathf.Sign(input);
    }
    void GatherWallGrabBuffer()
    {
        if(!isGrounded && X != 0f)
        {
            switch (X)
            {
                case > 0f:
                    wallGrabBufferR = playerVars.wallGrabBuffer;
                    break;
                case < 0f:
                    wallGrabBufferL = playerVars.wallGrabBuffer;
                    break;
            }
        }
    }
    void GatherBubbleInput()
    {
        float X = playerControls.Movement.Move.ReadValue<Vector2>().x;
        float Y = playerControls.Movement.Move.ReadValue<Vector2>().y;
        if(X == 0 && Y == 0) return;
        bubbleX = X;
        bubbleY = Y;
    }
    public void SetCanMove(bool can)
    {
        canMove = can;
        rb.velocity = Vector2.zero;
        
        X = 0;
        if (can)
        {
            SwitchGravity(gravityState.Normal);
            gameObject.tag = "Player";
        }
        else
        {
            SwitchGravity(gravityState.Space);
            gameObject.tag = "Dead";
        }
    }
    void ChangeSpring()
    {
        if (distJoint.frequency != playerVars.springFrequency)
            distJoint.frequency = playerVars.springFrequency;
        if (distJoint.dampingRatio != playerVars.springDamping)
            distJoint.dampingRatio = playerVars.springDamping;
    }
    void Timer()
    {
        SetCoyoteTime();

        JumpBufferTimer();
        SwingBufferTimer();

        TouchGroundScript();

        CoyoteTimeTimer();

        AntiDoubleJumpMechanism();
        AntiDoubleGrabMechanism();
        JumpCutMechanism();

        WallJumpBufferTimer();
        WallGrabBufferTimer();

        SwingBoosterChecker();
    }
    void SwingBoosterChecker()
    {
        if(isSwinging && transform.position.y < swingTransform.position.y - playerVars.exitBarHeight && !swingBoosterCheck)
        {
            swingBoosterCheck = true;
        }
    }
    void JumpCutMechanism()
    {
        if (isGrounded || isSwinging)
            jumpCut = false;
    }
    void SwingBufferTimer()
    {
        if (lastSwingPressed > 0)
            lastSwingPressed -= Time.deltaTime;
    }
    void SetCoyoteTime()
    {
        if (isGrounded && !(rb.velocity.y > 0))
            lastGroundedTime = playerVars.coyoteTime;
    }
    void JumpBufferTimer()
    {
        if (lastJumpPressed > 0)
            lastJumpPressed -= Time.deltaTime;
    }
    void TouchGroundScript()
    {
        if (isGrounded && !isSwinging)
        {
            movement = BasicMovement;
            jump = Jump;
            SwitchGravity(gravityState.Normal);
            StopCoroutine(JumpThightenerTimer());
        }
    }
    void CoyoteTimeTimer()
    {
        if (!isGrounded)
            lastGroundedTime -= Time.deltaTime;
    }
    void WallGrabBufferTimer()
    {
        if(wallGrabBufferL > 0f)
            wallGrabBufferL -= Time.deltaTime;
        if(wallGrabBufferR > 0f)
            wallGrabBufferR -= Time.deltaTime;
    }
    void WallJumpBufferTimer()
    {
        if (!isGrabbingProperSide() && wallJumpBuffer > 0f)
            wallJumpBuffer -= Time.deltaTime;
    }
    void AntiDoubleJumpMechanism()
    {
        if (jumped && !isGrounded)
            jumped = false;
    }
    void AntiDoubleGrabMechanism()
    {
        if (isWallPauseJumping && !onAnyWall)
            isWallPauseJumping = false;
    }
    public void SetupVariablesAfterDeath()
    {
        distJoint.enabled = false;
        lineRenderer.enabled = false;
        SwitchGravity(gravityState.Normal);
        isSwinging = false;
        swingBoosterCheck = false;
        rb.rotation = 0f;
        movement = BasicMovement;
        jump = Jump;
        ZeroAllBuffers();
    }

    void ZeroAllBuffers()
    {
        lastJumpPressed = 0f;
        wallJumpBuffer = 0f;
        lastGroundedTime = 0;
    }

    #endregion


    #region Animation

    void DetectSideOfAnimation()
    {
        RunAnimation();


        if (X < 0f && isFacingRight)
        {
            isFacingRight = false;
            spriteRenderer.flipX = true;
            cameraFollowScript.CallTurn();
        }
        else if (X > 0f && !isFacingRight)
        {
            isFacingRight = true;
            spriteRenderer.flipX = false;
            cameraFollowScript.CallTurn();
        }
    }
    void RunAnimation()
    {
        if (X != 0f && isGrounded && !jumped)
        {
            playerAnims.ChangeAnimationState(AnimationState.Run_Player.ToString());
            //Debug.Log("yo 2");
        }
        else if (X == 0f && isGrounded && !jumped)
        {
            playerAnims.ChangeAnimationState(AnimationState.Idle_Player.ToString());
            //Debug.Log("yo 3");
        }
    }
    void SpawnJumpCircle()
    {
        Instantiate(JumpCircle, new Vector3(transform.position.x, transform.position.y - jumpCircleposition, transform.position.z), Quaternion.identity);
    }

    #endregion


    #region Tongue Renderer


    public Vector3 GetSwingPointPosition()
    {
        if (swingPointPosition != null)
            return swingPointPosition;
        else
            return Vector3.zero;
    }



    #endregion

}
