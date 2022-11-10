using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    private static PlayerMovement playerMovement;
    private static PlayerHealth playerHealth;
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
}
