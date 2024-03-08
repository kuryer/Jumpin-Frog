using UnityEngine;

public class Swing : MonoBehaviour
{


    [SerializeField] SwingVariable ActualSwing;
    [SerializeField] MovementStateVariable ActualState;
    [SerializeField] CircleCollider2D SwingCollider;
    [SerializeField] bool onCooldown;

    [Header("Redesign Detection")]
    [SerializeField] bool newDetection;
    [SerializeField] Rigidbody2DRuntimeValue rb;
    [SerializeField] Transform player;
    delegate void DetectionDelegate();
    DetectionDelegate Detection;
    bool playerInRange;

    [Header("Cooldown")]
    [SerializeField] bool worksWithCooldown;
    [SerializeField] float Cooldown;

    [Header("Gizmos")]
    [SerializeField] PlayerMovementVariables playerVariables;
    [SerializeField] float RayDistance;

    //tutaj jeszcze mo¿emy dodawaæ cooldowny albo (design idea) stworzyæ oddzielny swing z cooldownem i jeden bez

    private void Start()
    {
        player = rb.Item.gameObject.transform;
        Detection = EnterDetection;
        enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (newDetection)
        {
            if (collision.CompareTag("Player"))
            {
                enabled = true;
                playerInRange = true;
                EnterDetection();
            }
        }
        else
        {
            if (collision.CompareTag("Player") && ActualState.Value is InAirMovementState)
            {
                SetActualSwing(this);
                SetCanSwing(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (newDetection)
        {
            if (collision.CompareTag("Player"))
            {
                if(ActualState.Value is SwingMovementState)
                {
                }
                else
                {
                    PlayerExitedSwingArea();
                    enabled = false;
                }
                playerInRange = false;
            }
        }
        else
        {
            if (collision.CompareTag("Player") && !(ActualState.Value is SwingMovementState) && ActualSwing.Value == this)
            {
                SetCanSwing(false);
                SetActualSwing(null);
            }
        }
    }

    private void OnDisable()
    {
        playerInRange = false;
        Detection = EnterDetection;
    }

    private void Update()
    {
        if(!playerInRange && ActualState.Value is not SwingMovementState)
        {
            PlayerExitedSwingArea();
            enabled = false;
        }
        Detection();
    }

    void EnterDetection()
    {
        if (player.position.y < transform.position.y && ActualState.Value is InAirMovementState)
            PlayerEnteredSwingArea();
    }

    void PlayerEnteredSwingArea()
    {
        Detection = ExitDetection;
        SetActualSwing(this);
    }

    void ExitDetection()
    {
        if (ActualState.Value is SwingMovementState)
            return;

        if(player.position.y >= transform.position.y || ActualState.Value is not InAirMovementState)
            PlayerExitedSwingArea();
    }

    void PlayerExitedSwingArea()
    {
        Detection = EnterDetection;
        SetActualSwing(null);
    }

    public void SetUsed()
    {
        if (worksWithCooldown)
            SetCooldown();
        else 
            Disable();
    }

    void Disable()
    {
        Detection = ExitDetection;
        SetActualSwing(null);
    }

    void SetCooldown()
    {

    }

    #region Obsolete
    void SetActualSwing(Swing actualSwing)
    {
        ActualSwing.Value = actualSwing;
    }

    void SetCanSwing(bool canSwing)
    {
        ActualSwing.CanSwing = canSwing;
    }
    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, SwingCollider.radius * transform.localScale.x);
        Gizmos.DrawLine(new Vector3(transform.position.x - (RayDistance / 2), transform.position.y - playerVariables.swingCatchYPosRedirection, transform.position.z),
            new Vector3(transform.position.x + (RayDistance / 2), transform.position.y - playerVariables.swingCatchYPosRedirection, transform.position.z));
    }

    #endregion

    #region Cooldown



    #endregion
}