using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueRenderer : MonoBehaviour
{
    [SerializeField] Sprite tongueSprite;
    [SerializeField] Vector3 tongueOffset;
    Vector3 swingPointPosition;


    void Start()
    {
        SetSprite();
    }

    void Update()
    {
        transform.rotation = Quaternion.FromToRotation(ModifiedPlayerPosition(), swingPointPosition);
    }


    #region Setup

    void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = tongueSprite;
    }

    #endregion


    #region Render

    Vector3 ModifiedPlayerPosition()
    {
        return swingPointPosition + tongueOffset;
    }

    void StartRender()
    {
        //animacja??
        //ustawic skale Y w zale¿noœci od odleg³osci gracza od punktu
    }

    #endregion
}
