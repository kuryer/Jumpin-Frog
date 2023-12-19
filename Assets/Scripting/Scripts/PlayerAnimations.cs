using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Animator animator;
    private string currentState;
    public string CurrentState
    {
        get { return currentState; }
    }

    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);
        //Debug.Log(newState);
        currentState = newState;
    }
}
