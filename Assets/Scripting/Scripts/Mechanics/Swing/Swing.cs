using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] SwingVariable ActualSwing;
    [SerializeField] MovementStateVariable ActualState;
    [SerializeField] CircleCollider2D SwingCollider;
    [SerializeField] bool onCooldown;
    [SerializeField] Transform player;
    //tutaj jeszcze mo¿emy dodawaæ cooldowny albo (design idea) stworzyæ oddzielny swing z cooldownem i jeden bez
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && ActualState.Value is InAirMovementState)
        {
            Debug.Log(collision.gameObject.name);
            player = collision.transform;
        }
    }

    private void Update()
    {
        if (player == null || onCooldown)
            return;

        if (player.position.y < transform.position.y)
            SetSwing(true);
        else if (ActualSwing.Value != null && ActualState.Value is InAirMovementState)
            SetSwing(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            ResetSwing();
    }

    void ResetSwing()
    {
        SetSwing(false);
        if(onCooldown)
            RenewSwing();
    }
    void SetSwing(bool toActive)
    {
        if (toActive)
            ActualSwing.Value ??= this;
        else
            ActualSwing.Value = null;
    }
    void RenewSwing()
    {
        onCooldown = false;
    }

    public void OnPlayerRelease()
    {
        onCooldown = true;
        SetSwing(false);
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, SwingCollider.radius * transform.localScale.x);
    }

    #endregion
}