using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DestinationPointsData))]
public class DestinationPointsDataEditor : Editor
{
    
    DestinationPointsData thisScript;
    SerializedProperty destinationPoints;

    private void OnEnable()
    {
        thisScript = (DestinationPointsData)target;
        destinationPoints = serializedObject.FindProperty("destinationPoints");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.DrawDefaultInspector();

        if (GUILayout.Button("Add Point"))
        {
            AddPoint();
        }
        if (GUILayout.Button("Delete Last Point"))
        {
            DeleteLastPoint();
        }

        serializedObject.ApplyModifiedProperties();
    }

    void AddPoint()
    {
        GameObject addedPoint = Instantiate(thisScript.destinationPointPrefab, thisScript.transform.parent);

        destinationPoints.arraySize++;
        destinationPoints.GetArrayElementAtIndex(destinationPoints.arraySize - 1).objectReferenceValue = addedPoint;

    }

    void DeleteLastPoint()
    {
        var toDelete = destinationPoints.GetArrayElementAtIndex(destinationPoints.arraySize - 1).objectReferenceValue;
        destinationPoints.DeleteArrayElementAtIndex(destinationPoints.arraySize - 1);
        DestroyImmediate(toDelete, true);
    }
}
