using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    private static GameManagerScript instance;

    [SerializeField] CanvasScript canvasScript;
    [SerializeField] LevelTransitionAnimationScript TransitionScript;
    enum TransitionAnimationState
    {
        CloseToLoadingScreen,
        OpenFromLoadingScreen,
        LoadingInactive
    }
    bool isPaused = false;
    bool canOpenPauseMenu;
    int currentLevel;
    int desiredLevel;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canOpenPauseMenu)
        {
            TogglePauseMenu();
        }
    }

    #region PauseMenu
    public void TogglePauseMenu()
    {
        //Sprawdzam czy ju¿ spauzowa³em czy jeszcze nie i manipuluje czasem w zale¿noœci od stanu Pause Menu
        if (!isPaused)
        {
            SetPause();
        }
        else
        {
            SetResume();
        }


        //W³¹czam Pause Menu wizualnie w Canvasie
        canvasScript.TogglePauseMenu(isPaused);
    }
    void SetPause()
    {
        Time.timeScale = 0;
        isPaused = true;
    }
    void SetResume()
    {
        Time.timeScale = 1;
        isPaused = false;
        canvasScript.TogglePauseMenu(isPaused);
    }
    #endregion

    //To jest pierwsza funkcja przez animacj¹. Przycisk -> GameManager
    public void LoadNextLevel(int levelIndex)
    {
        SetDesiredLevel(levelIndex);
        SetMenus();
        StartLevelTransition();
    }

    public void SetNewLevelTransition(LevelTransitionAnimationScript transitionScript)
    {
        TransitionScript = transitionScript;
    }
    //Ta metoda zaczyna animacje do zamkniecia przejscia
    void StartLevelTransition()
    {
        TransitionScript.ChangeAnimation(TransitionAnimationState.CloseToLoadingScreen.ToString());
    }

    void SetDesiredLevel(int levelIndex)
    {
        desiredLevel = levelIndex;
    }

    //To jest wywo³ane z zakonczonej animacji
    public void StartLoadingNextScene()
    {
        LevelLoader.LoadScene(desiredLevel);
        currentLevel = desiredLevel;
    }

    void SetMenus()
    {
        canvasScript.ToggleMenus(desiredLevel);
        canOpenPauseMenu = SetPauseMenuBool();
        SetResume();
    }
    bool SetPauseMenuBool()
    {
        bool canOpenPauseMenu = true;
        if (desiredLevel == 0) canOpenPauseMenu = false;
        return canOpenPauseMenu;
    }
}
