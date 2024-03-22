using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(LethalCollision))]
public class LethalCollisionEditor : Editor
{

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        LethalCollision script = (LethalCollision)target;
        DrawDefaultInspector();
        script.killType = (LethalCollision.DeathType)EditorGUILayout.EnumPopup("Death Type", script.killType);
        
    }





}
