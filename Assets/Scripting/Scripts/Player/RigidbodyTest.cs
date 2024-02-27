using UnityEngine;

public class RigidbodyTest : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerVarsSO playerVariables;
    [SerializeField] TestVariables testVariables;
    [SerializeField] BoolVariable isOnSlopeVariable;
    PlayerControls playerControls;
    Rigidbody2D platformRB;
    float X;
    float velocity;
    bool StandOnSlope;

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
        RBMovement();
    }

    void Update()
    {
        X = GatherInput();
        if (Input.GetKeyDown(KeyCode.R))
            rb.velocity = X * 5f * Vector2.right;
    }

    float GatherInput()
    {
        float input = playerControls.Movement.Move.ReadValue<Vector2>().x;
        if (input == 0) return 0;
        else return Mathf.Sign(input);
    }

    void RBMovement()
    {
        if (X == 0)
            SlowDownVelocity();
        else
            SetVelocityAcc();
        if (StandOnSlope)
            return;
        float velocityX = platformRB is null ? velocity : velocity + platformRB.velocity.x;
        rb.velocity = new Vector2(velocityX, rb.velocity.y);
    }

    void SlowDownVelocity()
    {
        if (StandOnSlope)
            return;

        if(!StandOnSlope && isOnSlopeVariable.Value)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
            StandOnSlope = true;
            return;
        }

        if (velocity == 0)
            return;

        if (Mathf.Abs(velocity + (testVariables.decc * -Mathf.Sign(velocity) * Time.fixedDeltaTime)) > 0)
            velocity += (testVariables.decc * -Mathf.Sign(velocity) * Time.fixedDeltaTime);
        else velocity = 0;
    }

    void SetVelocityAcc()
    {
        if (velocity == testVariables.maxSpeed * X)
            return;

        if (StandOnSlope)
            BackToDynamic();

        float multiplier = Mathf.Sign(velocity) == X ? testVariables.acc : testVariables.decc;

        if (Mathf.Abs(velocity + (multiplier * X * Time.fixedDeltaTime)) < testVariables.maxSpeed)
            velocity += (multiplier * X * Time.fixedDeltaTime);
        else velocity = testVariables.maxSpeed * X;
    }

    void BackToDynamic()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        StandOnSlope = false;
    }
}
