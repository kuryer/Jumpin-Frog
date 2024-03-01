using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Player/Movement States/In Bubble Movement")]
public class BubbleState : MovementState
{
    [SerializeField] DisablerEvent BubbleDisablerEvent;
    public override void OnEnter()
    {
        BubbleDisablerEvent.SetScripts(true);
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }
    public override void OnExit()
    {
        BubbleDisablerEvent.SetScripts(false);
    }
}
