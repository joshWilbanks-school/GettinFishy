using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FishingLine : MonoBehaviour, ILiner
{
    [SerializeField] GameObject start, end;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField, Range(0, .02f)] float lineWidth = .025f;
    [SerializeField] Color color = Color.black;
    private void Awake()
    {

        if (lineRenderer == null)
            TryGetComponent(out lineRenderer);

        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

    }

    void UpdateLineSettings()
    {

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        SetLineEnds();
    }

    void SetLineEnds()
    {
        start = GameObject.FindGameObjectWithTag("RodTip");
        end = GameObject.FindGameObjectWithTag("Hook");

        if (start != null)
        {
            float x = start.transform.position.x;
            float y = start.transform.position.y;
            lineRenderer.SetPosition(0, new Vector3(x, y, 0));
        }

        if (end != null)
        {
            float x = end.transform.position.x;
            float y = end.transform.position.y;
            lineRenderer.SetPosition(1, new Vector3(x, y, 0));
        }
    }

    private void Update()
    {

        SetLineEnds();
    }

    private void OnValidate()
    {
        UpdateLineSettings();
    }
}
