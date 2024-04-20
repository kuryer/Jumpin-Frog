using System.Collections;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] SwingVariable ActualSwing;
    [SerializeField] MovementStateVariable ActualState;
    [SerializeField] Rigidbody2DRuntimeValue rb;
    [SerializeField] Transform player;
    delegate void DetectionDelegate();
    DetectionDelegate Detection;
    bool playerInRange;
    [SerializeField] float playerAngle;

    [Header("Cooldown")]
    [SerializeField] bool worksWithCooldown;
    [SerializeField] float cooldown;
    bool isOnCooldown;

    [Header("Gizmos")]
    [SerializeField] CircleCollider2D SwingCollider;
    [SerializeField] PlayerMovementVariables playerVariables;
    [SerializeField] float RayDistance;
    Color onRight;
    Color onLeft;

    [Header("Animation")]
    [SerializeField] Animator animator;
    string actualAnimation;

    //tutaj jeszcze mo¿emy dodawaæ cooldowny albo (design idea) stworzyæ oddzielny swing z cooldownem i jeden bez

    private void Start()
    {
        player = rb.Item.gameObject.transform;
        Detection = EnterDetection;
        enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(ActualState.Value is not SwingMovementState && !isOnCooldown)
            {
                enabled = true;
                EnterDetection();
            }
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(ActualState.Value is not SwingMovementState && !isOnCooldown)
            {
                PlayerExitedSwingArea();
                enabled = false;
            }
            playerInRange = false;
        }
    }

    private void OnDisable()
    {
        Detection = EnterDetection;
    }

    private void Update()
    {
        Detection();
        CalculateAngle();
    }

    void CalculateAngle()
    {
        playerAngle = Vector2.Angle(player.position - transform.position, Vector2.right);
    }


    void EnterDetection()
    {
        if (ActualSwing.Value == null && player.position.y < transform.position.y && ActualState.Value is InAirMovementState)
            PlayerEnteredSwingArea();
    }

    void PlayerEnteredSwingArea()
    {
        Detection = ExitDetection;
        ChangeAnimation("InRange");
        SetActualSwing(this);
    }

    void ExitDetection()
    {
        if (ActualState.Value is SwingMovementState)
            return;

        if(ActualSwing.Value == this && (player.position.y >= transform.position.y || ActualState.Value is not InAirMovementState))
            PlayerExitedSwingArea();
    }

    void UsedDetection()
    {
        if (player.position.y >= transform.position.y || ActualState.Value is not InAirMovementState)
            PlayerExitedUsedArea();
    }

    void PlayerExitedUsedArea()
    {
        Detection = EnterDetection;
        ChangeAnimation("OutOfRange");
    }

    void PlayerExitedSwingArea()
    {
        Detection = EnterDetection;
        ChangeAnimation("OutOfRange");
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
        if (playerInRange)
        {
            Detection = UsedDetection;
            ChangeAnimation("Cooldown");
            SetActualSwing(null);
        }
        else
        {
            PlayerExitedSwingArea();
            enabled = false;
        }
    }

    void SetCooldown()
    {
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        PlayerExitedSwingArea();
        ChangeAnimation("Cooldown");
        enabled = false;
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        ChangeAnimation("OutOfRange");
        if (playerInRange)
            enabled = true;
        isOnCooldown = false;
    }

    void SetActualSwing(Swing actualSwing)
    {
        ActualSwing.Value = actualSwing;
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        SetDebugColors();
        Gizmos.DrawWireSphere(transform.position, SwingCollider.radius * transform.localScale.x);
        
        DrawDetectionArea();
        DrawHangDetection();
        DrawRedirectionBar();
    }

    void SetDebugColors()
    {
        try
        {
            if (!playerInRange)
            {
                onLeft = Color.white;
                onRight = Color.white;
                return;
            }
            Vector2 dir = player.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (playerVariables.IsInLeftSwingJumpArea(angle))
                onLeft = playerVariables.inSwingJumpAreaColor;
            else
                onLeft = Color.white;
            
            if (playerVariables.IsInRightSwingJumpArea(angle))
                onRight = playerVariables.inSwingJumpAreaColor;
            else
                onRight = Color.white;
        }
        catch
        {
            onLeft = Color.white;
            onRight = Color.white;
        }
    }

    void DrawDetectionArea()
    {
        if (!playerVariables.showSwingJumpArea)
            return;

        DrawRightDetection();
        DrawLeftDetection();
    }

    void DrawRedirectionBar()
    {
        if (!playerVariables.showRedirectionBar)
            return;
        Gizmos.DrawLine(new Vector3(transform.position.x - (RayDistance / 2), transform.position.y - playerVariables.swingCatchYPosRedirection, transform.position.z),
            new Vector3(transform.position.x + (RayDistance / 2), transform.position.y - playerVariables.swingCatchYPosRedirection, transform.position.z));
    }

    void DrawRightDetection()
    {
        Gizmos.color = onRight;
        Vector3 upperAngledPos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (playerVariables.upperDetectionAreaAngle)),
            Mathf.Sin(Mathf.Deg2Rad * (playerVariables.upperDetectionAreaAngle)));
        upperAngledPos *= SwingCollider.radius;
        Gizmos.DrawLine(transform.position, transform.position + upperAngledPos);
        Vector3 angledPos =new Vector3(Mathf.Cos(Mathf.Deg2Rad * (-playerVariables.DetectionAreaAngle)),
             Mathf.Sin(Mathf.Deg2Rad * (-playerVariables.DetectionAreaAngle)));
        angledPos *= SwingCollider.radius;
        Gizmos.DrawLine(transform.position, transform.position + angledPos);
        Gizmos.color = Color.white;
    }

    void DrawLeftDetection()
    {
        Gizmos.color = onLeft;
        Vector3 upperAngledPos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (-playerVariables.upperDetectionAreaAngle)),
            Mathf.Sin(Mathf.Deg2Rad * (-playerVariables.upperDetectionAreaAngle)));
        upperAngledPos *= SwingCollider.radius;

        Gizmos.DrawLine(transform.position, transform.position - upperAngledPos);
        Vector3 angledPos = new Vector3(-Mathf.Cos(Mathf.Deg2Rad * (-playerVariables.DetectionAreaAngle)),
             Mathf.Sin(Mathf.Deg2Rad * (-playerVariables.DetectionAreaAngle)));
        angledPos *= SwingCollider.radius;
        Gizmos.DrawLine(transform.position, transform.position + angledPos);
        Gizmos.color = Color.white;
    }

    void DrawHangDetection()
    {
        if (!playerVariables.showSwingHangArea)
            return;
        DrawRightHangDetection();
        DrawLeftHangDetection();
    }

    void DrawRightHangDetection()
    {
        Vector3 hangPos = new Vector3(transform.position.x, transform.position.y - playerVariables.swingHangBar);
        Vector3 angledPos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (playerVariables.rightHangAngle)),
             Mathf.Sin(Mathf.Deg2Rad * (-playerVariables.rightHangAngle)));
        Vector3 hangPosDestination = new Vector3(-playerVariables.swingHangBar / Mathf.Tan(Mathf.Deg2Rad * -playerVariables.leftHangAngle), playerVariables.swingHangBar);
        
        Gizmos.DrawLine(hangPos, transform.position - hangPosDestination);
        Gizmos.DrawLine(transform.position - hangPosDestination, transform.position + (angledPos * SwingCollider.radius));
    }

    void DrawLeftHangDetection()
    {
        Vector3 hangPos= new Vector3(transform.position.x, transform.position.y - playerVariables.swingHangBar);
        Vector3 angledPos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (-playerVariables.leftHangAngle)),
            Mathf.Sin(Mathf.Deg2Rad * (-playerVariables.leftHangAngle)));
        Vector3 hangPosDestination = new Vector3(playerVariables.swingHangBar / Mathf.Tan(Mathf.Deg2Rad * -playerVariables.leftHangAngle), playerVariables.swingHangBar);

        Gizmos.DrawLine(hangPos, transform.position - hangPosDestination);
        Gizmos.DrawLine(transform.position - hangPosDestination, transform.position + (angledPos * SwingCollider.radius));
    }

    #endregion

    #region Animation

    void ChangeAnimation(string newAnimationName)
    {
        if (newAnimationName.Equals(actualAnimation))
            return;
        animator.Play(newAnimationName);
        actualAnimation = newAnimationName;
    }

    #endregion
}