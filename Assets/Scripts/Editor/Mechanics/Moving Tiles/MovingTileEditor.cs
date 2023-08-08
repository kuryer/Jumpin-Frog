using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(MovingTile))]
public class MovingTileEditor : Editor
{
    MovingTile thsScript;
    SerializedProperty destinationPointsProperty;
    SerializedProperty destinationPointPrefab;

    private void OnEnable()
    {
        thsScript = (MovingTile)target;
        destinationPointsProperty = serializedObject.FindProperty("destinationPoints");
        destinationPointPrefab = serializedObject.FindProperty("destPointPrefab");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        serializedObject.ApplyModifiedProperties();
    }

    protected void AddPoint(GameObject prefab)
    {
        DestinationPoint addedPoint = Instantiate(prefab, thsScript.transform.parent).GetComponent<DestinationPoint>();

        destinationPointsProperty.arraySize++;
        destinationPointsProperty.GetArrayElementAtIndex(destinationPointsProperty.arraySize - 1).objectReferenceValue = addedPoint;
    }

    protected void DeleteLastPoint(GameObject prefab)
    {
        Debug.Log(prefab);
        Debug.Log(thsScript.transform.parent);
    }


}
