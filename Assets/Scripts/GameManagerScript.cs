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
    bool isAnimationTransitionClosed = false;
    bool canOpenPauseMenu;

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


    public void SetLoadingScreenAnimationFinished(bool isClosed)
    {
        isAnimationTransitionClosed = isClosed;
    }

    public async void LoadScene(int LevelIndex)
    {
        TransitionScript.ChangeAnimation(TransitionAnimationState.CloseToLoadingScreen.ToString());
        var scene = SceneManager.LoadSceneAsync(LevelIndex);
        scene.allowSceneActivation = false;
        //cast here animation to transition into loading screen

        do
        {
            await System.Threading.Tasks.Task.Delay(100);
        }while (scene.progress < .9f && isAnimationTransitionClosed);

        SetLoadingScreenAnimationFinished(false);
        TransitionScript.ChangeAnimation(TransitionAnimationState.OpenFromLoadingScreen.ToString());
        //await System.Threading.Tasks.Task.Delay(200);
        scene.allowSceneActivation = true;
    }
    public void LoadLevel(int LevelIndex)
    {
        canvasScript.ToggleMenus(LevelIndex);
        canOpenPauseMenu = true;
        LoadScene(LevelIndex);
    }
    public void LoadMainMenu()
    {
        //call animation
        canvasScript.ToggleMenus(0);
        canOpenPauseMenu = false;
        SetResume();

        LoadScene(0);
    }

}
