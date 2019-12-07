using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PolygonRenderer : MonoBehaviour
{
    public PolygonCollider2D polygonCollider;
    public LineRenderer lineRenderer;

    private void Update()
    {
        lineRenderer.positionCount = polygonCollider.points.Length;
        lineRenderer.SetPositions(polygonCollider.points.Select(x => (Vector3) x).ToArray());
    }
}
