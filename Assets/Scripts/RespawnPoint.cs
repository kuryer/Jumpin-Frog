using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private void Awake()
    {
        Helpers.PlayerHealth.SetRespawnPoint(transform.position);
    }
}
