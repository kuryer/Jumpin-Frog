using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : MovingTileEditor
{
    MovingTile inheritingScript;

    private void OnEnable()
    {
        inheritingScript = (MovingTile)target;
    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        if(GUILayout.Button("Add Point"))
        {
            inheritingScript.AddPoint();
        }
        if(GUILayout.Button("Delete Last Point"))
        {
            inheritingScript.DeleteLastPoint();
        }
    }
}
