using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    private static GameObject player;
    private static GameManagerScript gameManager;
    private static DataManagerScript dataManager;

    public static GameObject Player
    {
        get
        {
            if (player == null) player = GameObject.FindGameObjectWithTag("Player");
            return player;
        }
    }
    public static GameManagerScript GameManagerScript
    {
        get
        {
            if (gameManager == null) gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
            return gameManager;
        }
    }
    public static DataManagerScript DataManagerScript
    {
        get
        {
            if(dataManager == null) dataManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManagerScript>();
            return dataManager;
        }
    }
}
