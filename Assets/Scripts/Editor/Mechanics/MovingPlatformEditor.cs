using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : MovingTileEditor
{
    MovingPlatform thsScript;

    private void OnEnable()
    {
        thsScript = (MovingPlatform)target;
    }

    public override void OnInspectorGUI()
    {
        //serializedObject.Update();
        
        if(GUILayout.Button("Add Point"))
        {
            thsScript.AddPoint();
        }
        if(GUILayout.Button("Delete Last Point"))
        {
            thsScript.DeleteLastPoint();
        }
          
        base.DrawDefaultInspector();
        
        //serializedObject.ApplyModifiedProperties();
    }
}