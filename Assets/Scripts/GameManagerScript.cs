using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    private static GameManagerScript instance;

    [SerializeField] Mask transitionMask;
    [SerializeField] CanvasScript canvasScript;
    [SerializeField] LevelTransitionAnimationScript TransitionScript;
    [SerializeField] GameObject propPanel;
    enum TransitionAnimationState
    {
        CloseToLoadingScreen,
        OpenFromLoadingScreen,
        LoadingInactive
    }
    bool isPaused = false;
    bool isAnimationTransitionClosed = false;
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


    public void LoadNextLevel(int levelIndex)
    {
        SetDesiredLevel(levelIndex);
        StartLevelTransition();
    }


    void StartLevelTransition()
    {
        TransitionScript.ChangeAnimation(TransitionAnimationState.CloseToLoadingScreen.ToString());
    }

    void SetDesiredLevel(int levelIndex)
    {
        desiredLevel = levelIndex;
    }


    public void SetLoadingScreenAnimationFinished(bool isClosed)
    {
        isAnimationTransitionClosed = isClosed;
    }

    public void StartLoadingNextScene()
    {
        LoadScene();
    }
    async void LoadScene()
    {
        var scene = SceneManager.LoadSceneAsync(desiredLevel);
        //cast here animation to transition into loading screen

        if (scene.isDone)
        {
            currentLevel = desiredLevel;
        }
        //SetLoadingScreenAnimationFinished(false);
        //TransitionScript.ChangeAnimation(TransitionAnimationState.OpenFromLoadingScreen.ToString());
        //await System.Threading.Tasks.Task.Delay(200);
        FixTransition();
        await System.Threading.Tasks.Task.Delay(100);
        //CallTransitionOpenAnimation();
        Invoke("CallTransitionOpenAnimation", 2f);
    }
    public void LoadLevel()
    {
        canvasScript.ToggleMenus(desiredLevel);
        canOpenPauseMenu = true;
        LoadScene();
    }
    public void LoadMainMenu()
    {
        //call animation
        canvasScript.ToggleMenus(0);
        canOpenPauseMenu = false;
        SetResume();

        LoadScene();
    }
    void FixTransition()
    {
        // open prop -> turn off mask-> turn on mask-> turn off prop -> call open animation
        propPanel.SetActive(true);
        transitionMask.showMaskGraphic = true;
        transitionMask.showMaskGraphic = false;
        propPanel.SetActive(false);
    }
    void CallTransitionOpenAnimation()
    {
        TransitionScript.ChangeAnimation(TransitionAnimationState.OpenFromLoadingScreen.ToString());
    }
}
