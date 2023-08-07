using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MovingTile))]
public class MovingTileEditor : Editor
{
    //MovingTile thisScript;
    private void OnEnable()
    {
        //thisScript = (MovingTile)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("--------------------------------");
        base.OnInspectorGUI();
        /*
        if (GUILayout.Button("Add Point"))
        {
            thisScript.AddPoint();
        }
        if (GUILayout.Button("Delete Last Point"))
        {
            thisScript.DeleteLastPoint();
        }
        */
    }
}
