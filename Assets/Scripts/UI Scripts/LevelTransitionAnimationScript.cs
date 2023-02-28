using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitionAnimationScript : MonoBehaviour
{
    Animator animator;
    string currentState;


    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    public void AnimationFinished()
    {
        Helpers.GameManagerScript.SetLoadingScreenAnimationFinished(true);
        // wpisac zeby wyslal info o zmianie boola do game managera i zmienic te gowniana nazwe xdd
    }
    public void ChangeAnimation(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);
        //Debug.Log(newState);
        currentState = newState;
    }
}
