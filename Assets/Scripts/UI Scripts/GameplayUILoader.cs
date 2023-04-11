using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUILoader : MonoBehaviour
{
    private void Awake()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadSceneAsync("UI Scene", LoadSceneMode.Additive);        
        }
    }
}
