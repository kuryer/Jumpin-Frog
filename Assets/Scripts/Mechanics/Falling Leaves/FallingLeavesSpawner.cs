using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingLeavesSpawner : MonoBehaviour
{
    [Header("Spawn Variables")]
    [Range(0f, 15f)]
    [SerializeField] float SpawnAreaDistance;
    [Range(0f, 30f)]
    [SerializeField] float gizmosRayLength;
    [SerializeField] float minTimeToSpawn;
    [SerializeField] float maxTimeToSpawn;
    [Header("Delete Variables")]
    [SerializeField] LayerMask groundLayer;
    [Header("Leaf Variables")]
    [SerializeField] GameObject leafPrefab;
    [SerializeField] float leafLenght = 2f;

    float occupiedLeafX;

    private void OnEnable()
    {
        StartCoroutine(SpawnLeaf());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    #region Spawn Leaves

    IEnumerator SpawnLeaf()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(Random.Range(minTimeToSpawn, maxTimeToSpawn));
        }
    }

    void Spawn()
    {
        Instantiate(leafPrefab, GetLeafSpawnPosition(), Quaternion.identity);
    }

    Vector3 GetLeafSpawnPosition()
    {
        float randomX = Random.Range(transform.position.x - SpawnAreaDistance + (leafLenght/2), transform.position.x + SpawnAreaDistance - (leafLenght / 2));

        if (IsGeneratedPositionFree(randomX))
        {
            Vector3 newPos = new Vector3(randomX, transform.position.y, transform.position.z);
            occupiedLeafX = randomX;
            return newPos;
        }
        else
        {
            return GetLeafSpawnPosition();
        }
    }

    bool IsGeneratedPositionFree(float positionToCheck)
    {
        if(occupiedLeafX == 0)
        {
            return true;
        }
        if(positionToCheck > occupiedLeafX + (leafLenght/2) || positionToCheck < occupiedLeafX - (leafLenght / 2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion 

    #region Deleting Leaves

    private void FixedUpdate()
    {
        Vector2 origin = new Vector2(transform.position.x - SpawnAreaDistance, transform.position.y - gizmosRayLength);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right, SpawnAreaDistance * 2, groundLayer);

        if (hit.collider != null && hit.collider.CompareTag("Leaf"))
        {
            Destroy(hit.collider.gameObject);
            Debug.Log("yo");
        }
    }

    #endregion


    #region Gizmos

    private void OnDrawGizmos()
    {
        //Draws Leaves Spawn Range
        //////////////////////////
        Gizmos.color = Color.cyan;
        Vector3 cubeSize = new Vector3(SpawnAreaDistance * 2, 1f, 0f);
        Gizmos.DrawWireCube(transform.position, cubeSize);

        //////////////////
        // Draws Side Rays
        //////////////////
        Vector3 rayPos = new Vector3(cubeSize.x / 2, 0f, 0f);
        Vector3 rayLength = new Vector3(0f, rayPos.y - gizmosRayLength, 0f);
        Gizmos.DrawLine(transform.position - rayPos, transform.position - rayPos + rayLength);
        Gizmos.DrawLine(transform.position + rayPos, transform.position + rayPos + rayLength);

        ////////////////////////
        //Draws Real Spawn Range
        ////////////////////////
        Gizmos.color = new Color(0, 1, 1, .35f);
        Vector3 leafSizeV3 = new Vector3 (leafLenght, 0f, 0f);
        Gizmos.DrawCube(transform.position, cubeSize - leafSizeV3);

        ////////////////////////
        //Draws The Deleting Ray
        ////////////////////////
        Gizmos.color = Color.red;
        Vector3 from = new Vector3(transform.position.x - SpawnAreaDistance, transform.position.y - gizmosRayLength, 0);
        Vector3 to = new Vector3(transform.position.x + SpawnAreaDistance, transform.position.y - gizmosRayLength, 0);
        Gizmos.DrawLine(from, to);
    }

    #endregion
}
