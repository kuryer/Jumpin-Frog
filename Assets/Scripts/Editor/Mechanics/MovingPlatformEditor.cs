using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor
{
    //MovingPlatform thsScript;

    private void OnEnable()
    {
        //thsScript = (MovingPlatform)target;
    }

    public override void OnInspectorGUI()
    {
        //serializedObject.Update();
        /*
        if(GUILayout.Button("Add Point"))
        {
            thsScript.AddPoint();
        }
        if(GUILayout.Button("Delete Last Point"))
        {
            thsScript.DeleteLastPoint();
        }
        */
        base.DrawDefaultInspector();
        
        //serializedObject.ApplyModifiedProperties();
    }
}
[CustomEditor(typeof(MovingTile))]
public class MovingTileEditor : MovingPlatformEditor
{
    MovingTile thisScript;
    private void OnEnable()
    {
        thisScript = (MovingTile)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("--------------------------------");
        base.OnInspectorGUI();

        if (GUILayout.Button("Add Point"))
        {
            thisScript.AddPoint();
        }
        if (GUILayout.Button("Delete Last Point"))
        {
            thisScript.DeleteLastPoint();
        }
    }
}