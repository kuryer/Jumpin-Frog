using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Utilities/Variables/Specific/Direction Bool")]
public class DirectionBool : ScriptableObject
{
    public bool Left;
    public bool Right;

    public bool LeftAndRight()
    {
        return Left && Right;
    }

    public bool LeftOrRight() 
    {
        return Left || Right;
    }
}