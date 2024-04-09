using UnityEngine;

public class LethalCollision : MonoBehaviour
{
    [Header("New Controller")]
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
        if (ActualState.Value is not DeadState)
            DeathEvent.Raise();
    }
}
