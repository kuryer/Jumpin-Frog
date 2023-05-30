using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


[CustomEditor(typeof(MovingSpikes))]
public class MovingSpikesEditor : Editor
{
    ReorderableList points;
    SerializedProperty pointsList;
    MovingSpikes thisScript;

    private void OnEnable()
    {
        thisScript = (MovingSpikes)target;
        pointsList = serializedObject.FindProperty("points");
        points = new ReorderableList(serializedObject, pointsList, true, true, true, true);



        DrawListsHeader();
        DrawListsElements();
        OnAddPoint();
        OnRemovePoint();

    }
    void DrawListsHeader()
    {
        points.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Destination Points");
        };
    }

    void DrawListsElements()
    {
        points.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = points.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 270, EditorGUIUtility.singleLineHeight),
                element, GUIContent.none);
        };
    }
    void OnAddPoint()
    {
        points.onAddCallback = (points) =>
        {
            SpikesDestinationPoint point = Instantiate(thisScript.destinationPointPrefab, thisScript.transform.parent).GetComponent<SpikesDestinationPoint>();

            pointsList.arraySize++;
            pointsList.GetArrayElementAtIndex(pointsList.arraySize - 1).objectReferenceValue = point;
        };
    }

    void OnRemovePoint()
    {
        points.onRemoveCallback = (points) =>
        {
            DestroyImmediate(thisScript.points[points.index].gameObject, true);
            pointsList.DeleteArrayElementAtIndex(points.index);
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        base.DrawDefaultInspector();

        points.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}
