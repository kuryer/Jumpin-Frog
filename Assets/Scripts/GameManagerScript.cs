using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    private static GameManagerScript instance;

    [SerializeField] CanvasScript canvasScript;

    bool isPaused = false;
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
    }
    #endregion

    public async void LoadScene(int LevelIndex)
    {
        var scene = SceneManager.LoadSceneAsync(LevelIndex);
        scene.allowSceneActivation = false;

        //cast here animation to transition into loading screen

        do
        {
            await System.Threading.Tasks.Task.Delay(100);
        }while (scene.progress < .9f /* jeszcze bool odnosnie animacji*/);

        scene.allowSceneActivation = true;

    }
    public void LoadLevel(int LevelIndex)
    {
        SceneManager.LoadSceneAsync(LevelIndex);
        canvasScript.ToggleMenus(LevelIndex);
        canOpenPauseMenu = true;
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
        canvasScript.ToggleMenus(0);
        canOpenPauseMenu = false;
        SetResume();
    }

}
