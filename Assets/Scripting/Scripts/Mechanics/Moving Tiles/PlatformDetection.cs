using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDetection : MonoBehaviour
{
    [SerializeField] MovingPlatform platformScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Wpad³ w detection point");
        platformScript.CallNextPoint();
    }
}