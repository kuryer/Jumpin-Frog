using UnityEngine;

public class PlatformDetection : MonoBehaviour
{
    [SerializeField] MovingPlatform platformScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        platformScript.CallNextPoint();
    }
}