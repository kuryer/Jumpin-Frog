using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerMovement movementScript;
    PlayerAnimations animationScript;
    [SerializeField] Vector3 respawnPoint;
    float respawnSpeed; // <- JO TU MAM CU? TAKIEGO NI WIM CZY TEGO UZYC CZY NIE, ZEBY RAZ USTAWIAC TE PREDKOSC CZY NI
    [SerializeField] PlayerVarsSO playerVars;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    void Start()
    {
        movementScript = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }


    public void KillPlayer()
    {
        movementScript.SetupVariablesAfterDeath();
        animationScript.ChangeAnimationState("Death_Player");
        movementScript.SetCanMove(false);
        movementScript.enabled = false;
        Debug.Log("hello");
    }




    //gracz musi otrzymac info o tym gdzie jest miejsce odrodzenia
    // gracz musi miec oddzieln¹ funkcje pod smierc oraz jej animacje
    // potem przesun¹æ siê do miejsca odrodzenia
    // gracz musi miec oddzieln¹ funkcjê do odrodzenia sie i animacji
    /*
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
    */
}
