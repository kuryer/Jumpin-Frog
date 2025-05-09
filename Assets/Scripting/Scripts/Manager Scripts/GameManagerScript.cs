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
        Application.targetFrameRate = 60;
    }

    //To jest pierwsza funkcja przed animacj�. Przycisk -> GameManager
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
    }

    //To jest wywo�ane z zakonczonej animacji
    public void StartLoadingNextScene()
    {
        LevelLoader.LoadScene(desiredLevel);
        currentLevel = desiredLevel;
    }

    private void Update()
    {
        ChangeFramerate();
    }

    void ChangeFramerate()
    {
        if(Input.GetKeyDown(KeyCode.F1))
            Application.targetFrameRate = 60;
        if(Input.GetKeyDown(KeyCode.F2))
            Application.targetFrameRate = 144;
        if (Input.GetKeyDown(KeyCode.F3))
            Application.targetFrameRate = 240;
    }

    public void SetNewLevelTransition(LevelTransitionAnimationScript transitionScript)
    {
        TransitionScript = transitionScript;
    }

    public void Quit()
    {
        Debug.Log("Jaki� quit tej?");
        Application.Quit();
    }
}
