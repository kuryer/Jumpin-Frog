using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Player/Buffer")]
public class Buffer : ScriptableObject
{
    public CombinedBool ActivityInfo;
    public float MaxTime;
    public float ElapsedTime;
    public MovementStateVariable ActiveState;
    public MovementState ValidState;

    public void StartTimer()
    {
        ActivityInfo.FirstValue= true;
        ElapsedTime = MaxTime;
    }

    #region Courutine

    public IEnumerator StartTimerRoutine()
    {
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
    public void ElapseTime()
    {
        if(ElapsedTime > 0f)
        {
            ElapsedTime -= Time.deltaTime;
            return;
        }else
            ActivityInfo.FirstValue = false;
    }

    public void OnStateChangeCheck()
    {
        if (ActiveState.Value == ValidState)
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
