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
        Helpers.GameManagerScript.SetNewLevelTransition(gameObject.GetComponent<LevelTransitionAnimationScript>());
    }
    public void CloseAnimationFinished()
    {
        Helpers.GameManagerScript.StartLoadingNextScene();
    }
    public void OpenAnimationFinished()
    {

    }
    public void ChangeAnimation(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);
        currentState = newState;
    }
}
