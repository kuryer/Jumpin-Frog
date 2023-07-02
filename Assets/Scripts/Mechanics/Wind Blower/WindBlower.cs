using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlower : MonoBehaviour
{
    [Header("Gizmos")]
    [SerializeField] Transform gizmoPointRU;
    [SerializeField] Transform gizmoPointLU;
    [SerializeField] Transform gizmoPointRD;
    [SerializeField] Transform gizmoPointLD;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(gizmoPointRD.transform.position, gizmoPointRU.transform.position);
        Gizmos.DrawLine(gizmoPointLU.transform.position, gizmoPointLD.transform.position);
        Gizmos.DrawLine(gizmoPointLD.transform.position, gizmoPointRD.transform.position);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(gizmoPointLU.transform.position, gizmoPointRU.transform.position);

    }
}
