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
    [SerializeField] bool EveryStateValid;
    public List<MovementState> ValidStates;
    public Coroutine Coroutine;

    #region Courutine

    public IEnumerator StartTimerRoutine()
    {
        ActivityInfo.SetFirstValue(true);
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
        ActivityInfo.SetFirstValue(false);
    }

    #endregion

    public void OnStateChangeCheck()
    {
        if (EveryStateValid || ValidStates.Contains(ActiveState.Value))
            ActivityInfo.SecondValue = true;
        else
        {
            ActivityInfo.SecondValue = false;
            ResetTimer();
        }
    }
}