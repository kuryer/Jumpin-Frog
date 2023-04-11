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
        i operowa³ które poziomy mo¿na w³¹czyæ a które nie
        zmienia³ im animacje i opcje w³¹czenia ich st¹d
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
