using System.Collections;
using Unity.VisualScripting;
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
        Debug.Log("set swing: " + actualSwing);
        ActualSwing.Value = actualSwing;
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        SetDebugColors();
        Gizmos.DrawWireSphere(transform.position, SwingCollider.radius * transform.localScale.x);
        Gizmos.DrawLine(new Vector3(transform.position.x - (RayDistance / 2), transform.position.y - playerVariables.swingCatchYPosRedirection, transform.position.z),
            new Vector3(transform.position.x + (RayDistance / 2), transform.position.y - playerVariables.swingCatchYPosRedirection, transform.position.z));
        DrawDetectionArea();
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
            float angle = Vector2.Angle(player.position - transform.position, Vector2.right);
            if (angle > playerVariables.maxLeftAngle)
                onLeft = Color.cyan;
            else
                onLeft = Color.white;

            if (angle < playerVariables.maxRightAngle)
                onRight = Color.cyan;
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
        DrawRightDetection();
        DrawLeftDetection();
        DrawHangDetection();
    }

    void DrawRightDetection()
    {
        Gizmos.color = onRight;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(SwingCollider.radius, 0f));
        Vector3 angledPos =new Vector3(Mathf.Cos(Mathf.Deg2Rad * (-playerVariables.DetectionAreaAngle)),
             Mathf.Sin(Mathf.Deg2Rad * (-playerVariables.DetectionAreaAngle)));
        angledPos *= SwingCollider.radius;
        Gizmos.DrawLine(transform.position, transform.position + angledPos);
        Gizmos.color = Color.white;
    }

    void DrawLeftDetection()
    {
        Gizmos.color = onLeft;
        Gizmos.DrawLine(transform.position, transform.position - new Vector3(SwingCollider.radius, 0f));
        Vector3 angledPos = new Vector3(-Mathf.Cos(Mathf.Deg2Rad * (-playerVariables.DetectionAreaAngle)),
             Mathf.Sin(Mathf.Deg2Rad * (-playerVariables.DetectionAreaAngle)));
        angledPos *= SwingCollider.radius;
        Gizmos.DrawLine(transform.position, transform.position + angledPos);
        Gizmos.color = Color.white;
    }

    void DrawHangDetection()
    {
        DrawRightHangDetection();
        DrawLeftHangDetection();
    }

    void DrawRightHangDetection()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(SwingCollider.radius, 0f));
        Vector3 angledPos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (playerVariables.rightHangAngle)),
             Mathf.Sin(Mathf.Deg2Rad * (-playerVariables.rightHangAngle)));
        angledPos *= SwingCollider.radius;
        Gizmos.DrawLine(transform.position, transform.position + angledPos);
    }

    void DrawLeftHangDetection()
    {
        Gizmos.DrawLine(transform.position, transform.position - new Vector3(SwingCollider.radius, 0f));
        Vector3 angledPos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (-playerVariables.leftHangAngle)),
            Mathf.Sin(Mathf.Deg2Rad * (-playerVariables.leftHangAngle)));
        angledPos *= SwingCollider.radius;
        Gizmos.DrawLine(transform.position, transform.position + angledPos);
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