using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    private float X;
    private bool JumpDown;
    private bool JumpHold;
    private bool JumpUp;
    bool canMove = true;
    private float lastJumpPressed;
    Vector3 groundRayoffset;
    Vector3 wallRayoffset;


    [Header("Gravity")]
    [SerializeField] TextMeshProUGUI gravityStateText;
    enum gravityState
    {
        Normal,
        JumpTightener,
        Falling,
        WallGrab,
        Swing
    }
    gravityState GravityState;


    [Header("Movement")]
    [SerializeField] PlayerVarsSO playerVars;
    [SerializeField] TextMeshProUGUI velocityXText;
    [SerializeField] TextMeshProUGUI velocityYText;
    public delegate void MovementDelegate();
    public delegate void JumpDelegate();
    MovementDelegate movement;
    JumpDelegate jump;
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
    [SerializeField] float groundRayPosition;
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


    [Header("Swing")]
    SpringJoint2D distJoint;
    [SerializeField] bool canSwing;
    [SerializeField] float playerRotationFix;
    [SerializeField] float exitBarHeight;
    bool isSwinging;
    SwingPoint swingScript;
    Rigidbody2D swingPointRB;
    Transform swingTransform;
    float lastSwingPressed;
    bool SwingDown;
    bool SwingUp;
    bool swingBoosterCheck;
    bool swingJumped;
    float swingJumpBuffer;


    [Header("Line Renderer")]
    LineRenderer lineRenderer;
    Vector3 swingPointPosition;


    [Header("Animations")]
    PlayerAnimations playerAnims;
    enum AnimationState
    {
        Idle_Player,
        Run_Player,
        Jump_Player,
        Fall_Player,
        WallGrab_Player
    }
    SpriteRenderer spriteRenderer;


    #region Updates and Start

    void Start()
    {
        movement = BasicMovement;
        jump = Jump;
        rb = GetComponent<Rigidbody2D>();
        distJoint = GetComponent<SpringJoint2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        playerAnims = GetComponent<PlayerAnimations>();
        lineRenderer.widthMultiplier = playerVars.lineWidth;
        groundRayoffset = new Vector3(rayOffset, 0f, 0f);
        wallRayoffset = new Vector3(0f, wallRayOffset, 0f);
    }
    void Update()
    {
        GatherInput();
        FallChecker();

        jump();

        if (canSwing || isSwinging) Swing();
        if (isSwinging) LineRendering();
        if (canWallJump) WallJump();
        if(!isWallPauseJumping && !isSwinging) WallGrab();


        velocityXText.text = "Velocity x: " + rb.velocity.x;
        velocityYText.text = "Velocity x Swing: " + SwingSignVelocityX();

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
        if (canMove) movement();
        if (canCornerCorrect) CornerCorrect(rb.velocity.y);
        if (rb.velocity.y < 0f && !isGrounded && !isGrabbing) FallClamp();
    }
    #endregion


    #region Movements
    private void BasicMovement()
    {
        //calcualte the direction we want to move in and our desired velocity
        float maxSpeed = X * playerVars.moveSpeed;
        //calculate difference between current velocity and desired velocity
        float speedDif = maxSpeed - rb.velocity.x;
        //change acceleration rate depending on situation
        float accelRate = (Mathf.Abs(maxSpeed) > 0.01f) ? playerVars.acceleration : playerVars.decceleration;
        //applies acceleration to speed difference, the raises to a set power so acceleration increases with higher speeds
        //finally multiplies by sign to reapply direction
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, playerVars.velPower) * Mathf.Sign(speedDif);
            
        rb.AddForce(movement * Vector2.right);

        if (Mathf.Abs(rb.velocity.x) < 1f)
            rb.velocity = new Vector2(0f, rb.velocity.y);
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

        float speedDif = maxSpeed - (rb.velocity.magnitude * X);

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


    #region Jumps

    //===================
    //Normal Jump Section
    //===================
    
    private void Jump()
    {
        if (!jumped)
        {
            if(lastJumpPressed > 0 && isGrounded /* Jump Buffer */ || JumpDown && lastGroundedTime > 0 /* Coyote Time */)
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
        JumpCut();
    }
    void JumpCut()
    {
        if (rb.velocity.y > 0.1f && !JumpHold && !jumpCut)
        {
            //Debug.Log("Jump Cut Used");
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

            playerAnims.ChangeAnimationState(AnimationState.Jump_Player.ToString());
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
            Debug.Log("SwingJump");
            swingJumped = true;
            if(isSwinging)
                StopSwing();
            Debug.Log(rb.velocity.y);
            Vector2 direction = new Vector2(SwingJumpDirectionX() * X * playerVars.swingJumpBalance, SwingJumpDirectionY() * (1f - playerVars.swingJumpBalance) /* * Mathf.Sign(rb.velocity.y)*/);
            playerAnims.ChangeAnimationState(AnimationState.Jump_Player.ToString());
            float movePercent = MovePercentage();
            rb.velocity = Vector2.zero;
            rb.AddForce(direction * playerVars.swingJumpForce * movePercent, ForceMode2D.Impulse);
            ZeroAllBuffers();
            jump = WaitingJump;
        }
    }
    float SwingJumpDirectionX()
    {
        if (Mathf.Abs(rb.velocity.x) > 150f)
            return 150f;
        else
            return Mathf.Abs(rb.velocity.x);
    }
    float SwingJumpDirectionY()
    {
        if (rb.velocity.y < 0f)
            return 0f;
        if (rb.velocity.y > 150f)
            return 150f;
        return rb.velocity.y;
    }
    void AfterSwingJump()
    {
        if (JumpDown)
        {
            SwitchGravity(gravityState.Normal);
            playerAnims.ChangeAnimationState(AnimationState.Jump_Player.ToString());
            //Debug.Log("Swing Jump");
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * playerVars.afterSwingJumpPower, ForceMode2D.Impulse);
            ZeroAllBuffers();
            jump = Jump;
            JumpThightenerQueue();
        }
    }
    void WaitingJump()
    {
        if(isGrounded && !isSwinging)
        {
            jump = Jump;
        }
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
        if(isSwinging && SwingUp || isSwinging && transform.position.y >= swingTransform.position.y - exitBarHeight && swingBoosterCheck)
        {
            StopSwing();
        }
    }
    void StartSwing()
    {
        swingJumped = false;
        lastSwingPressed = 0f;
        distJoint.enabled = true;
        lineRenderer.enabled = true;
        SwitchGravity(gravityState.Swing);
        VelocityCut();
        isSwinging = true;
        swingScript.PlaySwingAnimation();
        movement = SwingingMovement;
        jump = SwingJump;
    }
    void StopSwing()
    {
        distJoint.enabled = false;
        lineRenderer.enabled = false;
        SwitchGravity(gravityState.Normal);
        swingScript.StartTimer();
        isSwinging = false;
        canSwing = false;
        rb.rotation = 0f;
        swingBoosterCheck = false;
        if(!swingJumped)
            StartCoroutine(SwingJumpBuffer());
        movement = InAirMovement;
        JumpThightenerQueue();
    }
    
    IEnumerator SwingJumpBuffer()
    {
        swingJumpBuffer = playerVars.SwingJumpBuffer;
        while(swingJumpBuffer > 0f)
        {
            swingJumpBuffer -= Time.deltaTime;
            yield return null;
        }
        SwingBoost();
    }
    void VelocityCut()
    {
        Debug.Log("Before Velocity Cut: " + rb.velocity);
        rb.velocity = new Vector2(rb.velocity.x / playerVars.swingVelocityCut, rb.velocity.y / playerVars.swingVelocityCut);
        Debug.Log("After Velocity Cut: " + rb.velocity);
    }

    void SwingBoost()
    {
        /*
        Vector2 transRight = new Vector2(playerVars.swingJumpBalance * rb.velocity.x, (1f - playerVars.swingJumpBalance) * rb.velocity.y);
        Vector2 direction = transRight * rb.velocity.magnitude;
        direction *= Mathf.Sign(XSwing);
        Debug.Log(Mathf.Sign(XSwing));
        rb.velocity = Vector2.zero;   ZASTANÓW SIE CO JEST LEPSZE (1.8 Z TYM VECTOR.ZERO 4 BEZ TEGO (SWING JUMP FORCE))
        */

        Debug.Log("SwingBoost");
        //Vector2 direction = new Vector2(playerVars.swingJumpBalance * rb.velocity.x, (1f - playerVars.swingJumpBalance) * rb.velocity.y);
        Vector2 direction = new Vector2(10f * X * playerVars.swingBoostBalance, 10f * (1f - playerVars.swingBoostBalance));
        playerAnims.ChangeAnimationState(AnimationState.Jump_Player.ToString());
        float movePercent = MovePercentage();
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * playerVars.swingBoostForce * movePercent, ForceMode2D.Impulse);
        jump = WaitingJump;
        //Debug.Log("magnitude: " + rb.velocity.magnitude.ToString() + /*", Transform.right " + transRight.ToString() + */", direction: " + direction.ToString());
    }
    float MovePercentage()
    {
        Debug.Log(rb.velocity.magnitude / playerVars.swingMoveSpeed);

        if ((rb.velocity.magnitude / playerVars.swingMoveSpeed) > playerVars.swingSpeedPercentage)
        {
            return Mathf.Abs(playerVars.swingMoveSpeed / playerVars.swingMoveSpeed);
        }
        else
        {
            return Mathf.Abs(rb.velocity.magnitude / playerVars.swingMoveSpeed);
        }
    }
    void LineRendering()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, swingPointPosition);
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
            rb.velocity = new Vector2(1f, 0f);
        }
        else if (onLeftWall && X <= 0f)
        {
            rb.velocity = new Vector2(-1f, 0f);
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
                    gravityStateText.text = "Gravity State: Normal";
                    break;
                }
            case gravityState.JumpTightener:
                {
                    rb.gravityScale = playerVars.jumpThightenerGravity;
                    GravityState = gravityState.JumpTightener;
                    gravityStateText.text = "Gravity State: Jump Thightener";
                    break;
                }
            case gravityState.Falling:
                {
                    //playerAnims.ChangeAnimationState(AnimationState.Fall_Player.ToString());
                    rb.gravityScale = playerVars.fallGravity;
                    GravityState = gravityState.Falling;
                    gravityStateText.text = "Gravity State: Falling";
                    break;
                }
            case gravityState.WallGrab:
                {
                    rb.gravityScale = playerVars.wallGravity;
                    GravityState = gravityState.WallGrab;
                    gravityStateText.text = "Gravity State: Wall Grab";
                    break;
                }
            case gravityState.Swing:
                {
                    rb.gravityScale = playerVars.swingGravity;
                    GravityState = gravityState.Swing;
                    gravityStateText.text = "Gravity State: Swing";
                    break;
                }
        }
    }
    /*
    private void JumpGravity()
    {
        if (isSwinging)
        {
            rb.gravityScale = playerVars.swingGravity;
            return;
        }
        if(IsFalling() && !isGrounded || !JumpHold && !isGrounded && !isWallJumping)
        {
            rb.gravityScale = gravityScale * playerVars.fallGravityMultiplier;
        }
        else if(!isGrabbing)
        {
            rb.gravityScale = gravityScale;
        }
    }
    bool IsFalling()
    {
        if (isGrounded || isGrabbing)
        {
            isFalling = false;
            return false;
        }
        if (rb.velocity.y > playerVars.jumpFallBorder && !isGrounded)
        {
            isFalling = true;
        }
        if (rb.velocity.y < playerVars.jumpFallBorder && isFalling && !isGrounded || rb.velocity.y < 0f)
        {
            return true;
        }
        return false;
    }
    */
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
        StopCoroutine(JumpThightenerTimer());
        StartCoroutine(JumpThightenerTimer());
    }
    void FallClamp()
    {
        playerAnims.ChangeAnimationState(AnimationState.Fall_Player.ToString());
        if (Mathf.Abs(rb.velocity.y) > playerVars.fallClampSpeed)
            rb.velocity = new Vector2(rb.velocity.x, -playerVars.fallClampSpeed);
    }
    #endregion


    #region Collision Check

    void CheckCollisions()
    {
        //Ground Collision
        isGrounded = Physics2D.Raycast(new Vector3(transform.position.x + groundRayPosition, transform.position.y, transform.position.z), Vector3.down, groundRayLength, groundLayer) || 
            Physics2D.Raycast(new Vector3(transform.position.x + groundRayPosition, transform.position.y, transform.position.z) - (2 * groundRayoffset), Vector3.down, groundRayLength, groundLayer) ||
            Physics2D.Raycast(new Vector3(transform.position.x + groundRayPosition, transform.position.y, transform.position.z) - groundRayoffset, Vector3.down, groundRayLength, groundLayer) ||
            Physics2D.Raycast(new Vector3(transform.position.x + groundRayPosition, transform.position.y, transform.position.z) + (2 * groundRayoffset), Vector3.down, groundRayLength, groundLayer) ||
            Physics2D.Raycast(new Vector3(transform.position.x + groundRayPosition, transform.position.y, transform.position.z) + groundRayoffset, Vector3.down, groundRayLength, groundLayer);

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
        Gizmos.DrawRay(new Vector3(transform.position.x + groundRayPosition, transform.position.y, transform.position.z), Vector3.down * groundRayLength);
        Gizmos.DrawRay(new Vector3(transform.position.x + groundRayPosition, transform.position.y, transform.position.z) - groundRayoffset, Vector3.down * groundRayLength);
        Gizmos.DrawRay(new Vector3(transform.position.x + groundRayPosition, transform.position.y, transform.position.z) - (2 * groundRayoffset), Vector3.down * groundRayLength);
        Gizmos.DrawRay(new Vector3(transform.position.x + groundRayPosition, transform.position.y, transform.position.z) + (2 * groundRayoffset), Vector3.down * groundRayLength);
        Gizmos.DrawRay(new Vector3(transform.position.x + groundRayPosition, transform.position.y, transform.position.z) + groundRayoffset, Vector3.down * groundRayLength);

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

    private void GatherInput()
    {
        JumpDown = Input.GetButtonDown("Jump");
        JumpHold = Input.GetButton("Jump");
        JumpUp = Input.GetButtonUp("Jump");
        SwingDown = Input.GetButtonDown("Swing");
        SwingUp = Input.GetButtonUp("Swing");
        X = XMovement();
        GatherWallGrabBuffer();
        if (JumpDown)
            lastJumpPressed = playerVars.jumpBuffer;
        if (SwingDown)
            lastSwingPressed = playerVars.swingBuffer;
        
    }
    void ChangeSpring()
    {
        if (distJoint.frequency != playerVars.springFrequency)
            distJoint.frequency = playerVars.springFrequency;
        if (distJoint.dampingRatio != playerVars.springDamping)
            distJoint.dampingRatio = playerVars.springDamping;
    }
    float XMovement()
    {
        float input = Input.GetAxisRaw("Horizontal");
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
        if(isSwinging && transform.position.y < swingTransform.position.y - exitBarHeight && !swingBoosterCheck)
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
    public void DeathCheck()
    {
        distJoint.enabled = false;
        lineRenderer.enabled = false;
        SwitchGravity(gravityState.Normal);
        isSwinging = false;
        swingBoosterCheck = false;
        rb.rotation = 0f;
        movement = BasicMovement;
        jump = Jump;
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


        if (X < 0f)
        {
            spriteRenderer.flipX = true;
        }
        else if (X > 0f)
        {
            spriteRenderer.flipX = false;
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

    #endregion
}
