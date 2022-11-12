using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingPoint : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    void Start()
    {
        arrow = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Helpers.PlayerMovement.SetCanSlingTrue(GetComponent<Rigidbody2D>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Helpers.PlayerMovement.SetCanSlingFalse();
        }
    }
}
