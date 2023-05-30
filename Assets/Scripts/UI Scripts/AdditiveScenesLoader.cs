using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveScenesLoader : MonoBehaviour
{
    private void Awake()
    {
        LoadGameManager();
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            LoadUIScene();
        }
    }

    void LoadUIScene()
    {
        SceneManager.LoadSceneAsync("UI Scene", LoadSceneMode.Additive);
    }
    public void LoadGameManager()
    {
        if(!SceneManager.GetSceneByName("GM Scene").isLoaded)
            SceneManager.LoadSceneAsync("GM Scene", LoadSceneMode.Additive);
    }
}
