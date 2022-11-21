using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingPoint : MonoBehaviour
{
    GameObject arrow;
    SpriteRenderer arrowRenderer;
    GameObject player;
    bool showArrow;
    bool canSling;
    void Start()
    {
        arrow = transform.GetChild(0).gameObject;
        arrowRenderer = arrow.GetComponent<SpriteRenderer>();
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(collision.transform.position.y < transform.position.y)
            {
                Helpers.PlayerMovement.SetCanSlingTrue(GetComponent<Rigidbody2D>(), gameObject.GetComponent<SlingPoint>());
                player = collision.gameObject;
                ShowArrow();
                showArrow = true;
            }
        }
    }
    */
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.transform.position.y < transform.position.y && !canSling)
            {
                Helpers.PlayerMovement.SetCanSlingTrue(GetComponent<Rigidbody2D>(), gameObject.GetComponent<SlingPoint>());
                player = collision.gameObject;
                ShowArrow();
                showArrow = true;
                canSling = true;
                Debug.Log("Show Arrow");
            }
            else if(collision.transform.position.y > transform.position.y && canSling)
            {
                Helpers.PlayerMovement.SetCanSlingFalse();
                HideArrow();
                showArrow = false;
                canSling = false;
                arrowRenderer.enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Helpers.PlayerMovement.SetCanSlingFalse();
            HideArrow();
            showArrow = false;
            canSling = false;
            arrowRenderer.enabled = false;
        }
    }
    private void Update()
    {
        if(showArrow) ArrowAnimation();
    }
    #region Animations

    void ShowArrow()
    {
        arrowRenderer.enabled = true;
    }

    public void HideArrow()
    {
        showArrow = false;
    }

    void ArrowAnimation()
    {
        Vector2 direction = arrow.transform.position - player.transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        arrow.transform.eulerAngles = new Vector3(0, 0, -angle);
    }

    #endregion
}
