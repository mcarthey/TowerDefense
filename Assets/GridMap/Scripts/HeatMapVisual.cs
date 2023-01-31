using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapVisual : MonoBehaviour
{
    private Grid _grid;
    private Mesh _mesh;
    private bool _updateMesh;

    private void Awake()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
    }
    public void SetGrid(Grid grid)
    {
        _grid = grid;
        UpdateHeatMapVisual();

        _grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, Grid.OnGridValueChangedEventArgs e)
    {
        //UpdateHeatMapVisual();
        _updateMesh = true;
    }

    private void LateUpdate()
    {
        if (_updateMesh)
        {
            _updateMesh = false;
            UpdateHeatMapVisual();
        }
    }

    private void UpdateHeatMapVisual()
    {
        MeshUtils.CreateEmptyMeshArrays(_grid.Height * _grid.Width, out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < _grid.Width; x++)
        {
            for (int y = 0; y < _grid.Height; y++)
            {
                var index = x * _grid.Height + y;
                Vector3 quadSize = new Vector3(1, 1) * _grid.CellSize;

                var gridValue = _grid.GetGridValue(x, y);
                float gridValueNormalized = (float)gridValue / Grid.HEAT_MAP_MAX_VALUE; // range 0-1
                Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);

                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, _grid.GetWorldPosition(x,y) + quadSize * 0.5f, 
                    0f, quadSize, gridValueUV, gridValueUV);
            }
        }

        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = triangles;
    }
}
