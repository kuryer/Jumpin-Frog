using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] SwingVariable ActualSwing;
    [SerializeField] MovementStateVariable ActualState;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.position.y < transform.position.y)
            if (collision.CompareTag("Player") && ActualState.Value is InAirMovementState)
                SetSwing();
    }

    void SetSwing()
    {
        ActualSwing.Value = this;
    }
}
