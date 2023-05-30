using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalCollision : MonoBehaviour
{
    public enum DeathType
    {
        SpikeDamage,
        FallDamage
        
    }
    public DeathType killType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerHealth>(out PlayerHealth player))
        {
            player.KillPlayer((int)killType);
        }
    }
}
