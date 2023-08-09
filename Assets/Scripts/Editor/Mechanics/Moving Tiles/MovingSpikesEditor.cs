using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MovingSpikes))]
public class MovingSpikesEditor : MovingTileEditor
{
    MovingTile parentScript;
    /*
    private override void OnEnable()
    {
        
        parentScript = (MovingTile)target;
    }
    */
    public override void OnInspectorGUI()
    {
    }
}
