using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    [SerializeField] GameObject button;


    void Start()
    {
        /*
        tutaj bede czytal informacje o skonczonych poziomach
        i operowa� kt�re poziomy mo�na w��czy� a kt�re nie
        zmienia� im animacje i opcje w��czenia ich st�d
        poprzez tablice albo mape czy cos takiego
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadLevel(int LevelIndex)
    {
        Helpers.GameManagerScript.LoadNextLevel(LevelIndex);
    }
}
