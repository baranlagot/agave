using System.Collections.Generic;
using UnityEngine;

public static class LineDrawer
{
    private static LineRenderer _lineRenderer;

    public static void DrawLine(Stack<Vector3> cellWorldPositions)
    {
        if (_lineRenderer == null)
        {
            _lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
            _lineRenderer.startWidth = 0.1f;
            _lineRenderer.endWidth = 0.1f;
        }

        _lineRenderer.positionCount = cellWorldPositions.Count;
        _lineRenderer.SetPositions(cellWorldPositions.ToArray());
    }

}