using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Variables/Specific/Combined Bool")]
public class CombinedBool : ScriptableObject
{
    public bool FirstValue;
    public bool SecondValue;

    public bool Value()
    {
        return FirstValue && SecondValue;
    }
}
