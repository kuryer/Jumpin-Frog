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
    Vector3 finalSwingPoint;
    delegate void RendererFunction();
    RendererFunction calculateFunction;

    void Start()
    {
        Setup();
    }

    private void Update()
    {
        calculateFunction();
    }


    #region Setup

    void Setup()
    {
        GetComponent<SpriteRenderer>().sprite = tongueSprite;
        lineRenderer = GetComponent<LineRenderer>();
        player = transform.parent.gameObject;
        calculateFunction = DontCalculatePoints;
    }

    public void SetSwingPointPosition(Vector3 swingPointPos)
    {
        swingPointPosition = swingPointPos;
    }


    #endregion


    #region Render

    Vector3 ModifiedPlayerPosition()
    {
        return transform.parent.position - tongueOffset;
    }

    public void TurnSpriteRenderer()
    {
        //animacja tu musi byæ jeszcze


        if (lineRenderer.enabled == false)
            lineRenderer.enabled = true;
        else
            lineRenderer.enabled = false;
    }

    public void ChangeRendererState()
    {
        if (calculateFunction == DontCalculatePoints)
            StartCalculation();
        else
            StopCalculation();
    }
    public void StartCalculation()
    {
        //animacja??
        //ustawic skale Y w zale¿noœci od odleg³osci gracza od punktu
        finalSwingPoint = ModifiedPlayerPosition();
        calculateFunction = CalculatePoints;
    }

    void CalculatePoints()
    {
        if(swingPointPosition != null)
        {
            lineRenderer.SetPosition(0, ModifiedPlayerPosition());
            lineRenderer.SetPosition(1, swingPointPosition);
        }
    }

    void DontCalculatePoints()
    {

    }

    public void StopCalculation()
    {
        //animacja
        ZeroPoints();
        calculateFunction = DontCalculatePoints;
    }

    void ZeroPoints()
    {
        lineRenderer.SetPosition(0, ModifiedPlayerPosition());
        lineRenderer.SetPosition(1, ModifiedPlayerPosition());
    }

    #endregion
}
