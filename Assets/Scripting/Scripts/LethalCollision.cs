using UnityEngine;

public class LethalCollision : MonoBehaviour
{
    [Header("New Controller")]
    [SerializeField] bool worksWithNewController;
    [SerializeField] GameEvent DeathEvent;
    [SerializeField] MovementStateVariable ActualState;
    [HideInInspector]public enum DeathType
    {
        SpikeDamage,
        FallDamage
        
    }
    [HideInInspector]public DeathType killType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (worksWithNewController && ActualState.Value is not DeadState)
            DeathEvent.Raise();
        else
            if (collision.TryGetComponent<PlayerHealth>(out PlayerHealth player))
            {
                player.KillPlayer((int)killType);
            }
    }
}
