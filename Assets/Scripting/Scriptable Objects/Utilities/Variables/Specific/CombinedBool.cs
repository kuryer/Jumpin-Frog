using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Utilities/Variables/Specific/Combined Bool")]
public class CombinedBool : ScriptableObject
{
    public bool FirstValue { get; private set; }
    public bool SecondValue;

    public void SetFirstValue(bool value)
    {
        FirstValue = value;
    }

    public bool Value()
    {
        return FirstValue && SecondValue;
    }

    public bool Or()
    {
        return FirstValue || SecondValue;
    }
}
