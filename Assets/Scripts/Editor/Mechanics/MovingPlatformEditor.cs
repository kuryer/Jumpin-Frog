using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor
{
    ReorderableList points;
    SerializedProperty pointsList;
    MovingPlatform thisScript;

    private void OnEnable()
    {
        thisScript = (MovingPlatform)target;
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
            //Lets manipulate the points' names so they'll be easier to work with 
            pointsList.arraySize++;
            pointsList.GetArrayElementAtIndex(pointsList.arraySize - 1).objectReferenceValue = point;
        };
    }

    void OnRemovePoint()
    {
        points.onRemoveCallback = (points) =>
        {
            GameObject toDelete = thisScript.points[points.index].gameObject;

            pointsList.DeleteArrayElementAtIndex(points.index);
            DestroyImmediate(toDelete, true);
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
