using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject levelSelectionMenu;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
