using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    private static GameManagerScript instance;

    [SerializeField] LevelTransitionAnimationScript TransitionScript;
    enum TransitionAnimationState
    {
        CloseToLoadingScreen,
        OpenFromLoadingScreen,
        LoadingInactive
    }
    int currentLevel = 0;
    int desiredLevel;
    int count;
    public int CurrentLevel { get { return currentLevel; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //To jest pierwsza funkcja przed animacj¹. Przycisk -> GameManager
    public void LoadNextLevel(int levelIndex)
    {
        SetDesiredLevel(levelIndex);
        StartLevelTransition();
    }
    void SetDesiredLevel(int levelIndex)
    {
        desiredLevel = levelIndex;
    }
    
    //Ta metoda zaczyna animacje do zamkniecia przejscia
    void StartLevelTransition()
    {
        TransitionScript.ChangeAnimation(TransitionAnimationState.CloseToLoadingScreen.ToString());
        if (currentLevel != 0) Helpers.PlayerMovement.SetCanMove(false);
    }

    //To jest wywo³ane z zakonczonej animacji
    public void StartLoadingNextScene()
    {
        LevelLoader.LoadScene(desiredLevel);
        currentLevel = desiredLevel;
    }

    public void SetNewLevelTransition(LevelTransitionAnimationScript transitionScript)
    {
        TransitionScript = transitionScript;
    }
}
