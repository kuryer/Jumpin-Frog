using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MovingSpikes))]
public class MovingSpikesEditor : MovingTileEditor
{
    MovingTile parentScript;

    private void OnEnable()
    {
        parentScript = (MovingTile)target;
    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        if (GUILayout.Button("Add Point"))
        {
            parentScript.AddPoint();
        }
        if (GUILayout.Button("Delete Last Point"))
        {
            parentScript.DeleteLastPoint();
        }
    }
}
