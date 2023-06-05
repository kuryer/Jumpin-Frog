using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafScript : MonoBehaviour
{
    [SerializeField] float fallingSpeed;

    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * fallingSpeed);
    }
}
