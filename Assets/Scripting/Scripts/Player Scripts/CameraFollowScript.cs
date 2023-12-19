using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField] float flipYRotationTime = 0.5f;
    PlayerMovement player;
    bool isFacingRight;

    private void Awake()
    {
        player = Helpers.PlayerMovement;
        playerTransform = player.transform;

        isFacingRight = player.isFacingRight;
    }

    void LateUpdate()
    {
        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        LeanTween.rotateY(gameObject, DetermineEndRotation(), flipYRotationTime).setEaseInOutSine();
    }

    float DetermineEndRotation()
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight)
        {
            return 0f;
        }
        else
        {
            return 180f;
        }
    }

}
