using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueRenderer : MonoBehaviour
{
    [SerializeField] Sprite tongueSprite;
    [SerializeField] Vector3 tongueOffset;
    Vector3 swingPointPosition;

    delegate void RendererFunction();
    RendererFunction rendererFunction;

    void Start()
    {
        SetSprite();
    }

    void Update()
    {
        rendererFunction();
    }


    #region Setup

    void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = tongueSprite;
    }

    void SetSwingPointPosition(Vector3 swingPointPos)
    {
        swingPointPosition = swingPointPos;
    }


    #endregion


    #region Render

    Vector3 ModifiedPlayerPosition()
    {
        return swingPointPosition + tongueOffset;
    }

    void ChangeRendererState()
    {
        if (rendererFunction == DontRender)
            StartRender();
        else
            StopRender();
    }
    void StartRender()
    {
        //animacja??
        //ustawic skale Y w zale¿noœci od odleg³osci gracza od punktu
        rendererFunction = Render;
    }

    void Render()
    {
        transform.rotation = Quaternion.FromToRotation(ModifiedPlayerPosition(), swingPointPosition);
    }

    void DontRender()
    {

    }

    void StopRender()
    {
        //animacja
        rendererFunction = DontRender;
    }





    #endregion
}
