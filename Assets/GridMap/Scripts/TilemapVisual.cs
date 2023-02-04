using System;
using System.Collections.Generic;
using Assets.GridMap.Scripts.CodeMonkey.Utils;
using UnityEngine;
using static CodeMonkey.Utils.World_Mesh;

namespace Assets.GridMap.Scripts
{
    public class TilemapVisual : MonoBehaviour
    {
        private Grid<Tilemap.TilemapObject> _grid;
        private Mesh _mesh;
        private bool _updateMesh;

        [SerializeField] private TilemapSpriteUV[] _tilemapSpriteUvArray;
        private Dictionary<Tilemap.TilemapObject.TilemapSprite, UVCoordinates> _uvCoordinatesDictionary;

        public void SetGrid(Tilemap tilemap, Grid<Tilemap.TilemapObject> grid)
        {
            _grid = grid;
            UpdateHeatMapVisual();

            _grid.OnGridValueChanged += Grid_OnGridValueChanged;
            tilemap.OnLoaded += Tilemap_OnLoaded;
        }

        private void Tilemap_OnLoaded(object sender, EventArgs e)
        {
            _updateMesh = true;
        }

        private void Awake()
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;

            Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
            float textureWidth = texture.width;
            float textureHeight = texture.height;

            // convert from pixels into normalized values
            _uvCoordinatesDictionary = new Dictionary<Tilemap.TilemapObject.TilemapSprite, UVCoordinates>();

            foreach (TilemapSpriteUV tilemapSpriteUV in _tilemapSpriteUvArray)
            {
                _uvCoordinatesDictionary[tilemapSpriteUV.TilemapSprite] = new UVCoordinates()
                {
                    uv00 = new Vector2(tilemapSpriteUV.uv00Pixels.x / textureWidth, tilemapSpriteUV.uv00Pixels.y / textureHeight),
                    uv11 = new Vector2(tilemapSpriteUV.uv11Pixels.x / textureWidth, tilemapSpriteUV.uv11Pixels.y / textureHeight),
                };
            }
        }

        private void Grid_OnGridValueChanged(object sender, Grid<Tilemap.TilemapObject>.OnGridValueChangedEventArgs e)
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
            MeshUtils.CreateEmptyMeshArrays(_grid.Height * _grid.Width, out var vertices, out var uv, out var triangles);

            for (var x = 0; x < _grid.Width; x++)
            {
                for (var y = 0; y < _grid.Height; y++)
                {
                    var index = x * _grid.Height + y;
                    var quadSize = new Vector3(1, 1) * _grid.CellSize;

                    var gridObject = _grid.GetGridObject(x, y);
                    var tilemapSprite = gridObject.GetTilemapSprite();

                    Vector2 gridUV00, gridUV11;
                    if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.None)
                    {
                        gridUV00 = Vector2.zero;
                        gridUV11 = Vector2.zero;
                        quadSize = Vector3.zero;
                    }
                    else
                    {
                        UVCoordinates uvCoordinates = _uvCoordinatesDictionary[tilemapSprite];
                        gridUV00 = uvCoordinates.uv00;
                        gridUV11 = uvCoordinates.uv11;
                    }

                    MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, _grid.GetWorldPosition(x, y) + quadSize * 0.5f,
                        0f, quadSize, gridUV00, gridUV11);
                }
            }

            _mesh.vertices = vertices;
            _mesh.uv = uv;
            _mesh.triangles = triangles;
        }

        [Serializable]
        public struct TilemapSpriteUV
        {
            public Tilemap.TilemapObject.TilemapSprite TilemapSprite;
            public Vector2Int uv00Pixels;
            public Vector2Int uv11Pixels;
        }

        public struct UVCoordinates
        {
            public Vector2 uv00;
            public Vector2 uv11;
        }
    }
}
