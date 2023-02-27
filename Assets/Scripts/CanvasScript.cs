using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject PauseMenu;

    private static GameObject instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TogglePauseMenu(bool isPaused)
    {
        if (isPaused)
        {
            PauseMenu.SetActive(true);
        }
        else
        {
            PauseMenu.SetActive(false);
        }
    }


    public void ToggleMenus(int LevelIndex)
    {
        if(LevelIndex == 0)
        {
            MainMenu.SetActive(true);
            PauseMenu.SetActive(false);
        }
        else if(LevelIndex > 0)
        {
            MainMenu.SetActive(false);
        }
    } 

}
