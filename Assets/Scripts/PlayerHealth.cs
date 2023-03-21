using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerMovement movementScript;
    [SerializeField] Transform respawnPoint;
    [SerializeField] PlayerVarsSO playerVars;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    void Start()
    {
        movementScript = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    //gracz musi otrzymac info o tym gdzie jest miejsce odrodzenia
    // gracz musi miec oddzieln¹ funkcje pod smierc oraz jej animacje
    // potem przesun¹æ siê do miejsca odrodzenia
    // gracz musi miec oddzieln¹ funkcjê do odrodzenia sie i animacji
    public void PlayerDeath()
    {
        StartCoroutine(RespawnTimer());
    }

    IEnumerator RespawnTimer()
    {
        movementScript.SetupDeadPlayer();
        spriteRenderer.enabled = false;
        movementScript.enabled = false;
        transform.position = respawnPoint.position;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(playerVars.respawnTime);
        spriteRenderer.enabled = true;
        movementScript.enabled = true;
    }
    public void SetRespawnPoint(Transform newRespawnPoint)
    {
        if (newRespawnPoint == respawnPoint) return;

        respawnPoint = newRespawnPoint;
    }
}
