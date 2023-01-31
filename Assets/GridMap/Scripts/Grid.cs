using System;
using CodeMonkey.Utils;
using UnityEngine;

public class Grid
{
    public const int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VALUE = 0;

    private readonly int[,] _gridArray;
    private readonly Vector3 _originPosition;

    public int Height { get; }

    public int Width { get; }

    public float CellSize { get; }

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        Width = width;
        Height = height;
        CellSize = cellSize;
        _originPosition = originPosition;

        _gridArray = new int[width, height];

        bool showDebug = false;

        if (showDebug)
        {
            var debugTextArray = new TextMesh[width, height];

            for (var x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (var y = 0; y < _gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = UtilsClass.CreateWorldText(_gridArray[x, y].ToString(), null,
                        GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, // move from origin to middle of grid cell
                        20, Color.white, TextAnchor.MiddleCenter);

                    // left-side vertical line
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    // bottom horizontal
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }

            // top horizontal
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.blue, 100f);
            // right vertical
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 100f);

            OnGridValueChanged += (sender, eventArgs) => { debugTextArray[eventArgs.X, eventArgs.Y].text = _gridArray[eventArgs.X, eventArgs.Y].ToString(); };
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        var vector2Int = GetXY(worldPosition);
        return GetGridValue(vector2Int.x, vector2Int.y);
    }

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

    public void SetValue(Vector3 worldPosition, int value)
    {
        var vector2Int = GetXY(worldPosition);
        SetGridValue(vector2Int.x, vector2Int.y, value);
    }

    public void AddValue(int x, int y, int value)
    {
        SetGridValue(x, y, GetGridValue(x, y) + value);
    }

    public void AddValue(Vector3 worldPosition, int value, int fullValueRange, int totalRange)
    {
        int lowerValueAmount = Mathf.RoundToInt((float) value / (totalRange - fullValueRange));

        var origin = GetXY(worldPosition);
        for (int x = 0; x < totalRange; x++)
        {
            for (int y = 0; y < totalRange - x; y++) // the larger the x becomes, the smaller the y
            {
                int radius = x + y;
                int addValueAmount = value;
                if (radius > fullValueRange)
                {
                    addValueAmount -= lowerValueAmount * (radius - fullValueRange);
                }

                AddValue(origin.x + x, origin.y + y, addValueAmount); // upper right triangle

                if (x != 0) // prevent duplicating the center where the triangles join
                {
                    AddValue(origin.x - x, origin.y + y, addValueAmount); // upper left triangle
                }
                if (y != 0)
                {
                    AddValue(origin.x + x, origin.y - y, addValueAmount); // lower right triangle

                    if (x != 0)
                    {
                        AddValue(origin.x - x, origin.y - y, addValueAmount); // lower left triangle
                    }
                }

            }
        }
    }

    public int GetGridValue(int x, int y)
    {
        if (x >= 0 && x < Width &&
            y >= 0 && y < Height)
        {
            return _gridArray[x, y];
        }

        return -1;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * CellSize + _originPosition;
    }

    private Vector2Int GetXY(Vector3 worldPosition)
    {
        var x = Mathf.FloorToInt((worldPosition - _originPosition).x / CellSize);
        var y = Mathf.FloorToInt((worldPosition - _originPosition).y / CellSize);

        return new Vector2Int(x, y);
    }

    private void SetGridValue(int x, int y, int value)
    {
        if (x >= 0 && x < Width &&
            y >= 0 && y < Height)
        {
            _gridArray[x, y] = Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE);
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs
            { X = x, Y = y });

            //_debugTextArray[x, y].text = _gridArray[x, y].ToString(); <-- changed to use event
        }
    }

    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int X;
        public int Y;
    }
}
