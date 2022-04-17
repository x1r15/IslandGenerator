using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PolygonBuilder : MonoBehaviour
{
    [SerializeField]
    private float _minRadius;

    [SerializeField]
    private float _maxRadius;

    [SerializeField]
    private float _minDegreesPerStep;

    [SerializeField]
    private float _maxDegreesPerStep;

    [SerializeField]
    private TileBase _grassTile;
    
    [SerializeField]
    private Tilemap _tilemap;

    private LineRenderer _lineRenderer;
    private PolygonCollider2D _collider;

    private List<Vector3> _vertices;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _collider = GetComponent<PolygonCollider2D>();
    }

    private Vector3 GenerateVertex(Vector2 center, float rotation, float distanceFromCenter)
    {
        var line = new Vector2(distanceFromCenter, 0);
        return center + line.Rotate(rotation);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GeneratePolygon();
            DrawTiles();
        }
    }

    private void GeneratePolygon()
    {
        _vertices = new List<Vector3>();
        var center = transform.position;
        var totalRotation = 0f;
        while (true)
        {
            totalRotation += Random.Range(_minDegreesPerStep, _maxDegreesPerStep);
            if (totalRotation >= 360) break;
            var distanceFromCenter = Random.Range(_minRadius, _maxRadius);
            _vertices.Add(GenerateVertex(center, totalRotation, distanceFromCenter));
        }

        _collider.points = _vertices.Select(v => (Vector2)v).ToArray();
        _lineRenderer.positionCount = _vertices.Count;
        _lineRenderer.SetPositions(_vertices.ToArray());
    }

    private void DrawTiles()
    {
        _tilemap.ClearAllTiles();
        var checkSize = 0.2f;
        var boundsInfo = _collider.bounds;
        var bottomLeft = boundsInfo.center - boundsInfo.extents;
        var topRight = boundsInfo.center + boundsInfo.extents;
        for (var y = bottomLeft.y; y < topRight.y; y += checkSize)
        {
            for (var x = bottomLeft.x; x < topRight.x; x += checkSize)
            {
                if (_collider.OverlapPoint(new Vector3(x, y, 0))) {
                    _tilemap.SetTile(_tilemap.WorldToCell(new Vector3(x, y, 0)), _grassTile);
                }
            }
        }
    }
}
