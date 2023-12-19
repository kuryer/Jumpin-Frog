using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    Animator animator;
    bool isCollected = false;
    enum AnimationState
    {
        Coin_Idle,
        Coin_Collect
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCollected)
        {
            CollectCoin();
        }
    }

    void CollectCoin()
    {
        animator.Play(AnimationState.Coin_Collect.ToString());
        isCollected = true;
        Helpers.DataManagerScript.AddCoin();
    }

    public void DeleteCoin()
    {
        Destroy(gameObject);
    }

}
