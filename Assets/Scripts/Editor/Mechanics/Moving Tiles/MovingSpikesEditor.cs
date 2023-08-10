using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MovingSpikes))]
public class MovingSpikesEditor : MovingTileEditor
{
    MovingSpikes thisScript;
    SerializedProperty destinationPoints;

    public override void OnEnable()
    {
        base.OnEnable();
        destinationPoints = serializedObject.FindProperty("destinationPoints");
        thisScript = (MovingSpikes)target;
    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        /*
        serializedObject.Update();
        if (GUILayout.Button("Add Point"))
        {
            AddPoint(thisScript.destinationPointPrefab, thisScript.transform.parent, destinationPoints);
        }
        if (GUILayout.Button("Delete Last Point"))
        {
            DeleteLastPoint(thisScript.destinationPointPrefab, thisScript.transform.parent, destinationPoints);
        }
        serializedObject.ApplyModifiedProperties();
        */
    }
}
