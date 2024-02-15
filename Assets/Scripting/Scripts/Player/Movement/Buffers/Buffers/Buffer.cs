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

    #region Courutine

    public IEnumerator StartTimerRoutine()
    {
        ElapsedTime = MaxTime;
        ActivityInfo.FirstValue = true;
        while (ElapsedTime > 0f)
        {
            ElapsedTime -= Time.deltaTime;
            yield return null;
        }
        ActivityInfo.FirstValue = false;
    }

    public void RenewTimer()
    {
        ElapsedTime = MaxTime;
    }

    #endregion
    public void OnStateChangeCheck()
    {
        if (ValidStates.Contains(ActiveState.Value))
            ActivityInfo.SecondValue = true;
        else
        {
            ActivityInfo.SecondValue = false;
            ZeroTimer();
        }
    }

    public void ZeroTimer()
    {
        ElapsedTime = 0f;
        ActivityInfo.FirstValue = false;
    }
}
