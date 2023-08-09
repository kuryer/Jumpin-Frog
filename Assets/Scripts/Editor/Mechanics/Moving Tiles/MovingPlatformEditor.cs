using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : MovingTileEditor
{
    MovingPlatform thisScript;
    SerializedProperty destinationPoints;

    public override void OnEnable()
    {
        base.OnEnable();
        destinationPoints = serializedObject.FindProperty("destinationPoints");
        thisScript = (MovingPlatform)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.DrawDefaultInspector();
        
        if(GUILayout.Button("Add Point"))
        {
            AddPoint(thisScript.destinationPointPrefab, thisScript.transform.parent, destinationPoints);
        }
        if(GUILayout.Button("Delete Last Point"))
        {
            DeleteLastPoint(thisScript.destinationPointPrefab, thisScript.transform.parent, destinationPoints);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
