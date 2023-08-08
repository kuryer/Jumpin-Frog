using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : MovingTileEditor
{
    MovingPlatform thisScript;

    private void OnEnable()
    {
        thisScript = (MovingPlatform)target;
    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        
        if(GUILayout.Button("Add Point"))
        {
            AddPoint(thisScript.destinationPointPrefab);
        }
        if(GUILayout.Button("Delete Last Point"))
        {
            DeleteLastPoint(thisScript.destinationPointPrefab);
        }
        
    }
}
