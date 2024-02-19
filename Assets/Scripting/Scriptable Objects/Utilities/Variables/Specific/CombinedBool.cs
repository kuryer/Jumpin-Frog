using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Utilities/Variables/Specific/Combined Bool")]
public class CombinedBool : ScriptableObject
{
    public bool FirstValue;
    public bool SecondValue;

    public bool Value()
    {
        return FirstValue && SecondValue;
    }

    public bool Or()
    {
        return FirstValue || SecondValue;
    }
}
