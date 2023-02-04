using Assets.GridMap.Scripts.CodeMonkey.Utils;
using UnityEngine;

namespace Assets.GridMap.Scripts
{
    public class HeatMapGenericVisual : MonoBehaviour
    {
        private Grid<HeatMapGridObject> _grid;
        private Mesh _mesh;
        private bool _updateMesh;

        private void Awake()
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
        }
        public void SetGrid(Grid<HeatMapGridObject> grid)
        {
            _grid = grid;
            UpdateHeatMapVisual();

            _grid.OnGridValueChanged += Grid_OnGridValueChanged;
        }

        private void Grid_OnGridValueChanged(object sender, Grid<HeatMapGridObject>.OnGridValueChangedEventArgs e)
        {
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

                    HeatMapGridObject gridObject = _grid.GetGridObject(x, y);
                    float gridValueNormalized = gridObject.GetValueNormalized();
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
}
