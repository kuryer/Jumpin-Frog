using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndDoorScript : MonoBehaviour
{
    [SerializeField] int loadLevel;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float rayLength = 1.62f;
    [SerializeField] float yRayOffset = .17f;
    bool isPlayerInExit;
    Vector2 colliderPosition;

    private void Awake()
    {
        colliderPosition = new Vector2(GetComponent<Collider2D>().bounds.center.x, GetComponent<Collider2D>().bounds.center.y - yRayOffset);
    }
    private void Update()
    {
        if (isPlayerInExit)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Pls remember to look at this script bcs its not finished / saving aint done");
                Helpers.GameManagerScript.LoadNextLevel(loadLevel);
                //zapisz przejscie tego poziomu
            }
        }
    }
    private void FixedUpdate()
    {
        CheckCollisions();
    }

    void CheckCollisions()
    {
        isPlayerInExit = Physics2D.Raycast(colliderPosition, Vector2.left, rayLength, playerLayer) ||
            Physics2D.Raycast(colliderPosition, Vector2.right, rayLength, playerLayer) ||
            Physics2D.Raycast(colliderPosition, Vector2.up, rayLength, playerLayer);
    }
    private void OnDrawGizmos()
    {
        Vector2 gizmosColliderPosition = new Vector2(GetComponent<Collider2D>().bounds.center.x, GetComponent<Collider2D>().bounds.center.y);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(new Vector3(gizmosColliderPosition.x, gizmosColliderPosition.y - yRayOffset, 0f), Vector3.left * rayLength);
        Gizmos.DrawRay(new Vector3(gizmosColliderPosition.x, gizmosColliderPosition.y - yRayOffset, 0f), Vector3.right * rayLength);
        Gizmos.DrawRay(new Vector3(gizmosColliderPosition.x, gizmosColliderPosition.y, 0f), Vector3.up * rayLength);
    }

}
