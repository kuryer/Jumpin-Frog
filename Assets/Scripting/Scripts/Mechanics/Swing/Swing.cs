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
            SetActualSwing(this);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !(ActualState.Value is SwingMovementState) && ActualSwing.Value == this)
            SetActualSwing(null);
    }
    void SetActualSwing(Swing actualSwing)
    {
        ActualSwing.Value = actualSwing;
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, SwingCollider.radius * transform.localScale.x);
    }

    #endregion
}