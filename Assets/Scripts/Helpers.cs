using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    private static PlayerMovement playerMovement;
    private static PlayerHealth playerHealth;
    private static GameManagerScript gameManager;

    public static PlayerMovement PlayerMovement
    {
        get
        {
            if (playerMovement == null) playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            return playerMovement;
        }
    }
    public static PlayerHealth PlayerHealth
    {
        get
        {
            if (playerHealth == null) playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
            return playerHealth;
        }
    }
    public static GameManagerScript GameManager
    {
        get
        {
            if (gameManager == null) gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
            return gameManager;
        }
    }
}
