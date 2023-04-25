using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueRenderer : MonoBehaviour
{
    [SerializeField] Sprite tongueSprite;
    [SerializeField] Vector3 tongueOffset;
    LineRenderer lineRenderer;
    GameObject player;
    Vector3 swingPointPosition;

    delegate void RendererFunction();
    RendererFunction rendererFunction;

    void Start()
    {
        Setup();
    }

    private void Update()
    {
        rendererFunction();
    }


    #region Setup

    void Setup()
    {
        GetComponent<SpriteRenderer>().sprite = tongueSprite;
        lineRenderer = GetComponent<LineRenderer>();
        player = transform.parent.gameObject;
        rendererFunction = DontRender;
    }

    public void SetSwingPointPosition(Vector3 swingPointPos)
    {
        lineRenderer.SetPosition(1, swingPointPos);
    }


    #endregion


    #region Render

    Vector3 ModifiedPlayerPosition()
    {
        return swingPointPosition + tongueOffset;
    }

    void TurnSpriteRenderer()
    {
        if (lineRenderer.enabled == false)
            lineRenderer.enabled = true;
        else
            lineRenderer.enabled = false;
    }

    public void ChangeRendererState()
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
        TurnSpriteRenderer();
    }

    void Render()
    {
        if(swingPointPosition != null)
        {
            /*
            Quaternion rotation = Quaternion.FromToRotation(ModifiedPlayerPosition(), swingPointPosition);
            transform.localRotation = rotation;
            */
            lineRenderer.SetPosition(0, transform.parent.position);
        }
    }

    void DontRender()
    {

    }

    void StopRender()
    {
        //animacja
        TurnSpriteRenderer();
        rendererFunction = DontRender;
    }





    #endregion
}
