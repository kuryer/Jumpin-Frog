using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    public static void LoadScene(int LevelIndex)
    {
        SceneManager.LoadScene(LevelIndex);
    }

}
