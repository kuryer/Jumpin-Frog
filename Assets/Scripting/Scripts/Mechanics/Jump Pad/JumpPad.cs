using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float jumpForce;
    [SerializeField] float cooldown;
    float timePassed;
    SpriteRenderer spriteRenderer;

    [Header("New Controller")]
    [SerializeField] bool worksWithNewController;
    [SerializeField] JumpPadControllerRuntimeValue JumpPadValue;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        ElapseTime();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (timePassed > 0f)
            return;
        if(collision != null && collision.gameObject.CompareTag("Player"))
        {
            if(worksWithNewController)
                JumpPadValue.Item.Jump(jumpForce);
            else
            {
                var playerMovement = collision.GetComponent<PlayerMovement>();
                playerMovement.OnJumpPad(jumpForce);
            }
            StartTimer();
        }
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
