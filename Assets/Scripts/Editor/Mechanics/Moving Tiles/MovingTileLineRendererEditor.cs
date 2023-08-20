using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MovingTileLineRenderer), true)]
public class MovingTileLineRendererEditor : Editor
{
    MovingTileLineRenderer thisScript;

    private void OnEnable()
    {
        thisScript = (MovingTileLineRenderer)target;
    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        if (GUILayout.Button("Reload Line Renderer"))
        {
            thisScript.ReloadLineRenderer();
        }
    }
}
