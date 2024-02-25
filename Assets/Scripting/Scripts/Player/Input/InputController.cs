using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    // THIS SCRIPT IS TEMPORARY FOR PROTOTYPING PURPOSES
    [SerializeField] FloatVariable X;


    public void OnMove(InputAction.CallbackContext context)
    {
        float x = context.ReadValue<Vector2>().x;
        if (x == 0)
        {
            X.Value = 0;
            return;
        }
        X.Value = Mathf.Sign(x);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
