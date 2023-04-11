using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    [Header("Pause Flip Control")]
    [SerializeField] GameObject pauseMenuObject;
    delegate void PauseFlipDelegate();
    PauseFlipDelegate FlipPause;
    private void Awake()
    {
        //read saved data and apply to the settings    
    }

    void Start()
    {
        SetupFlippingOnLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FlipPause?.Invoke();
        }
    }

    #region Pause Control

    void OpenPause()
    {
        Time.timeScale = 0f;
        pauseMenuObject.SetActive(true);
        FlipPause = ClosePause;
    }
    public void ClosePause()
    {
        Time.timeScale = 1.0f;
        pauseMenuObject.SetActive(false);
        FlipPause = OpenPause;
    }

    void SetupFlippingOnLevel()
    {
        if(Helpers.GameManagerScript.CurrentLevel != 0)
        {
            FlipPause = OpenPause;
        }
    }

    #endregion

    public void ExitLevel()
    {
        Time.timeScale = 1.0f;
        Helpers.GameManagerScript.LoadNextLevel(0);
    }

}
