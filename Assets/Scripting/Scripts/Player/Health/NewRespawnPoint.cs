using UnityEngine;

public class NewRespawnPoint : MonoBehaviour
{
    [Header("Respawn")]
    [SerializeField] bool isSet;
    [SerializeField] RespawnPointVariables RespawnInfo;
    [SerializeField] Collider2D Collider;
    [Header("Animator")]
    [SerializeField] Animator animator;

    private void Awake()
    {
        if (isSet)
            SetRespawn();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Collider.enabled)
            SetRespawn();
    }

    void SetRespawn()
    {
        RespawnInfo.SetRespawnPoint(this);
        Collider.enabled = false;
        animator.Play("RespawnPoint_Set");
    }

    public void ResetRespawn()
    {
        Collider.enabled = true;
        animator.Play("RespawnPoint_UnsetIdle");
    }
}
