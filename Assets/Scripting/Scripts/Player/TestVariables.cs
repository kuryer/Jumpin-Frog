using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Test Variables")]
public class TestVariables : ScriptableObject
{
    public float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float decceleration;
    [HideInInspector] public float acc { get { return maxSpeed * acceleration; } }
    [HideInInspector] public float decc { get { return maxSpeed * decceleration; } }
}
