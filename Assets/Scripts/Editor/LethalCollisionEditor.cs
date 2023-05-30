using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(LethalCollision))]
public class LethalCollisionEditor : Editor
{

    public override void OnInspectorGUI()
    {
        LethalCollision script = (LethalCollision)target;

        script.killType = (LethalCollision.DeathType)EditorGUILayout.EnumPopup("Death Type", script.killType);

    }





}
