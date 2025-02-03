using System.Collections.Generic;
using UnityEngine;

public static class LineDrawer
{
    private static LineRenderer _lineRenderer;

    public static void DrawLine(Stack<Vector3> cellWorldPositions, Color color)
    {
        if (_lineRenderer == null)
        {
            _lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
            _lineRenderer.startWidth = 0.4f;
            _lineRenderer.endWidth = 0.4f;
            _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }

        _lineRenderer.positionCount = cellWorldPositions.Count;
        _lineRenderer.SetPositions(cellWorldPositions.ToArray());
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }

}