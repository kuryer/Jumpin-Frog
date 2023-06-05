using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    MovingSpikes script;

    private void Awake()
    {
        script = GetComponent<MovingSpikes>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            playerMovement.SetPlatformTransform(script, true);
            //collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            playerMovement.SetPlatformTransform(script, false);
            //collision.transform.SetParent(null);
        }
    }


}
