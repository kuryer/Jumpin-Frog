using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("Jump Pad")]
    [SerializeField] JumpPadControllerRuntimeValue JumpPadValue;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float jumpForce;
    [SerializeField] float cooldown;
    [SerializeField] bool inRange;
    float timePassed;


    private void Update()
    {
        ElapseTime();
        if(timePassed <= 0 && inRange)
        {
            JumpPadValue.Item.Jump(jumpForce);
            StartTimer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            inRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            inRange = false;
    }

    void StartTimer()
    {
        timePassed = cooldown;
    }

    void ElapseTime()
    {
        if (timePassed > 0f)
        {
            timePassed -= Time.deltaTime;
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
}
