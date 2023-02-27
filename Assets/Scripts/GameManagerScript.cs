using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] CanvasScript canvasScript;

    private static GameObject instance;

    bool isPaused = false;
    bool canOpenPauseMenu;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        //Sprawdzam czy moge w og�le w��czy� Pause Menu
        if (!canOpenPauseMenu)
            return;

        //Sprawdzam czy ju� spauzowa�em czy jeszcze nie i manipuluje czasem w zale�no�ci od stanu Pause Menu
        if (!isPaused)
        {
            SetPause();
        }
        else
        {
            SetResume();
        }


        //W��czam Pause Menu wizualnie w Canvasie
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
