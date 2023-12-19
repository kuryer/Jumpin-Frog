using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject levelSelectionMenu;

    // Start is called before the first frame update
    void Start()
    {
        //if(SceneManager.sceneCount == 1)
        //SceneManager.LoadSceneAsync("GM Scene", LoadSceneMode.Additive);
    }


    #region Button Functions

    public void OpenLevelSelectionMenu()
    {
        DeactivateAll();
        levelSelectionMenu.SetActive(true);
    }

    public void OpenStartMenu()
    {
        DeactivateAll();
        startMenu.SetActive(true);
    }

    public void DeactivateAll()
    {
        startMenu.SetActive(false);
        levelSelectionMenu.SetActive(false);
    }

    #endregion


}
