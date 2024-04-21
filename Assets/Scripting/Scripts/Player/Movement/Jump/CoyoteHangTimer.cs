using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoyoteHangTimer : MonoBehaviour
{
    [SerializeField] float hangTime;
    [SerializeField] GravityControllerRuntimeValue gravityController;
    [SerializeField] GravityState coyoteHangGravity;
    [SerializeField] GravityState inAirGravity;
    Coroutine coroutine;

    IEnumerator HangTimer()
    {
        gravityController.Item.ChangeGravity(coyoteHangGravity);
        yield return new WaitForSeconds(hangTime);
        gravityController.Item.ChangeGravity(inAirGravity);
        enabled = false;
    }

    private void OnEnable()
    {
        coroutine = StartCoroutine(HangTimer());
    }

    private void OnDisable()
    {
        StopCoroutine(coroutine);
    }
}
