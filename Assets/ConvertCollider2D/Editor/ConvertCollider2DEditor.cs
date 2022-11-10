using UnityEditor;
using UnityEngine;

[UnityEditor.CustomEditor(typeof(ConvertCollider2D))]
public class ConvertCollider2DEditor : Editor {


    private bool loader;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var convertColliderComponent = (ConvertCollider2D)target;
        GUILayout.BeginHorizontal();
        if (loader)
        {
            if (convertColliderComponent.GetComponent<PolygonCollider2D>())
            {
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Convert to Edge Collider 2D"))
                {


                    ConvertCollider2DValue.polygonCollider2D = convertColliderComponent.GetComponent<PolygonCollider2D>().points;
                    DestroyImmediate(convertColliderComponent.GetComponent<PolygonCollider2D>());
                    convertColliderComponent.gameObject.AddComponent<EdgeCollider2D>();
                    convertColliderComponent.GetComponent<EdgeCollider2D>().points = ConvertCollider2DValue.polygonCollider2D;


                }


                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Remove Polygon Collider 2D"))
                {

                    DestroyImmediate(convertColliderComponent.GetComponent<PolygonCollider2D>());
                }

            }
            else
            {
                if (!convertColliderComponent.GetComponent<EdgeCollider2D>())
                {
                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button("Add Edge Collider 2D"))
                    {
                        convertColliderComponent.gameObject.AddComponent<PolygonCollider2D>();

                        ConvertCollider2DValue.polygonCollider2D = convertColliderComponent.GetComponent<PolygonCollider2D>().points;
                        DestroyImmediate(convertColliderComponent.GetComponent<PolygonCollider2D>());
                        convertColliderComponent.gameObject.AddComponent<EdgeCollider2D>();
                        convertColliderComponent.GetComponent<EdgeCollider2D>().points = ConvertCollider2DValue.polygonCollider2D;
                    }
                }
            }
        }
        if (convertColliderComponent.GetComponent<PolygonCollider2D>() && convertColliderComponent.GetComponent<EdgeCollider2D>())
        {

            loader = false;
            Debug.LogWarning("Please remove one of the Polygon Collider 2D or Edge Collider 2D components!"+ "\nПожалуйста уберите один из компонентов Polygon Collider 2D или Edge Collider 2D! ");
            GUI.backgroundColor = Color.red;

            if (GUILayout.Button("Remove Polygon Collider 2D"))
            {
           
                DestroyImmediate(convertColliderComponent.GetComponent<PolygonCollider2D>());
            }
          
            GUILayout.Label("Or");

            if (GUILayout.Button("Remove Edge Collider 2D"))
            {
                
                DestroyImmediate(convertColliderComponent.GetComponent<EdgeCollider2D>());
            }
        }
        else
        {
            loader = true;
        }


        if (loader)
        {

            if (convertColliderComponent.GetComponent<EdgeCollider2D>())
            {
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Convert to Polygon Collider 2D"))
                {


                    ConvertCollider2DValue.edgeCollider2D = convertColliderComponent.GetComponent<EdgeCollider2D>().points;
                    DestroyImmediate(convertColliderComponent.GetComponent<EdgeCollider2D>());
                    convertColliderComponent.gameObject.AddComponent<PolygonCollider2D>();
                    convertColliderComponent.GetComponent<PolygonCollider2D>().points = ConvertCollider2DValue.edgeCollider2D;

                }


                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Remove Edge Collider 2D"))
                {

                    DestroyImmediate(convertColliderComponent.GetComponent<EdgeCollider2D>());
                }
            }
            else
            {
                if (!convertColliderComponent.GetComponent<PolygonCollider2D>())
                {
                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button("Add Polygon Collider 2D"))
                    {
                        convertColliderComponent.gameObject.AddComponent<PolygonCollider2D>();
                    }
                }
            }
        }
        GUILayout.EndHorizontal();
    }
}

public static class ConvertCollider2DValue
{
    public static Vector2[] polygonCollider2D;
    public static Vector2[] edgeCollider2D;
}
