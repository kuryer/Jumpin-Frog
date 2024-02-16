using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Player/Buffer")]
public class Buffer : ScriptableObject
{
    public CombinedBool ActivityInfo;
    public float MaxTime;
    public float ElapsedTime;
    public MovementStateVariable ActiveState;
    public List<MovementState> ValidStates;
    public Coroutine Coroutine;

    #region Courutine

    public IEnumerator StartTimerRoutine()
    {
        ActivityInfo.FirstValue = true;
        while (ElapsedTime > 0f)
        {
            ElapsedTime -= Time.deltaTime;
            yield return null;
        }
        ResetTimer();
    }

    public void RenewTimer()
    {
        ElapsedTime = MaxTime;
    }
    public void ResetTimer()
    {
        ElapsedTime = MaxTime;
        ActivityInfo.FirstValue = false;
    }

    #endregion

    public void OnStateChangeCheck()
    {
        if (ValidStates.Contains(ActiveState.Value))
            ActivityInfo.SecondValue = true;
        else
        {
            ActivityInfo.SecondValue = false;
            ResetTimer();
        }
    }
}