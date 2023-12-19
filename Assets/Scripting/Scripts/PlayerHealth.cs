using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerHealth : MonoBehaviour
{
    PlayerMovement movementScript;
    PlayerAnimations animationScript;
    [SerializeField] GameObject respawnPointObject;
    [SerializeField] int coinsTakenPerDeath;
    Vector3 respawnPointPosition;
    float respawnSpeed; // <- JO TU MAM CUŒ TAKIEGO NI WIM CZY TEGO UZYC CZY NIE, ZEBY RAZ USTAWIAC TE PREDKOSC CZY NI
    [SerializeField] PlayerVarsSO playerVars;
    bool isDead = false;
    public UnityEvent OnPlayerDeath;


    void Start()
    {
        animationScript = GetComponent<PlayerAnimations>();
        movementScript = GetComponent<PlayerMovement>();
        respawnSpeed = playerVars.respawnMoveTowardSpeed;
    }


    #region Kill Player

    public void KillPlayer(int typeOfDmg)
    {
        if (!isDead)
        {
            OnPlayerDeath?.Invoke();
            movementScript.SetupVariablesAfterDeath();
            movementScript.SetCanMove(false);
            movementScript.enabled = false;
            PlayDeathAnimation(typeOfDmg);
            Helpers.DataManagerScript.SubtractCoins(coinsTakenPerDeath);
            Debug.Log("Player Died");
            SetIsDead(true);
        }
    }
    void PlayDeathAnimation(int typeOfDmg)
    {
        switch (typeOfDmg)
        {
            case 0:
                animationScript.ChangeAnimationState("Death_Player");
                break;
            case 1:
                animationScript.ChangeAnimationState("FallDeath_Player");
                break;
        }
    }

    public void StartMovingTowardRoutine()
    {
        StartCoroutine(MoveTowardRespawn());
    }

    IEnumerator MoveTowardRespawn()
    {
        yield return new WaitForSeconds(.6f);
        Vector3 respawnPosition = new Vector3(respawnPointPosition.x, respawnPointPosition.y, 0f);
        transform.position = respawnPosition;
        Respawn();
    }

    void Respawn()
    {
        animationScript.ChangeAnimationState("Respawn_Player");
    }

    public void AfterRespawnAnimation()
    {
        movementScript.enabled = true;
        movementScript.SetCanMove(true);
        SetIsDead(false);
    }

    void SetIsDead(bool state)
    {
        isDead = state;
    }

    #endregion


    #region Respawn Point

    public void SetRespawnPoint(GameObject newRespawnPoint, bool isFirst)
    {
        Vector3 newRespawnPointPos = newRespawnPoint.transform.position;
        if (newRespawnPointPos == respawnPointPosition) return;

        if (!isFirst)
        {
            ChangePreviousRespawnPoint();
        }

        respawnPointPosition = newRespawnPointPos;
        respawnPointObject = newRespawnPoint;
    }

    void ChangePreviousRespawnPoint()
    {
        if(respawnPointObject != null)
        {
            respawnPointObject.GetComponent<RespawnPoint>().ClearPreviousBool();
        }
    }

    #endregion
}
