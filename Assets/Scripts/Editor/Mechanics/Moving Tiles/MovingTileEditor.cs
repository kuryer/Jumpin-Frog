using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(MovingTile))]
public class MovingTileEditor : Editor
{
    MovingTile thsScript;
    SerializedProperty destinationPointsProperty;
    SerializedProperty destinationPointPrefab;

    public virtual void OnEnable()
    {
        thsScript = (MovingTile)target ?? throw new NullReferenceException();
        destinationPointsProperty = serializedObject.FindProperty("destinationPoints");
        destinationPointPrefab = serializedObject.FindProperty("destPointPrefab");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        serializedObject.ApplyModifiedProperties();
    }

    protected void AddPoint(GameObject prefab, Transform parentTransform, SerializedProperty points)
    {
        GameObject addedPoint = Instantiate(prefab, parentTransform);

        points.arraySize++;
        points.GetArrayElementAtIndex(points.arraySize - 1).objectReferenceValue = addedPoint;
       
    }

    protected void DeleteLastPoint(GameObject prefab, Transform parentTransform, SerializedProperty points)
    {
        var toDelete = points.GetArrayElementAtIndex(points.arraySize - 1).objectReferenceValue;
        points.DeleteArrayElementAtIndex(points.arraySize - 1);
        DestroyImmediate(toDelete, true);
    }


}
