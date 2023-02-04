using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Assets.GridMap.Scripts;
using UnityEngine;

public class Tilemap
{
    public event EventHandler OnLoaded;
    
    private readonly Grid<TilemapObject> _grid;

    public Tilemap(int width, int height, float cellSize, Vector3 originPosition)
    {
        _grid = new Grid<TilemapObject>(width, height, cellSize, originPosition,
            (g, x, y) => new TilemapObject(g, x, y));
    }

    public void SetTilemapSprite(Vector3 worldPosition, TilemapObject.TilemapSprite tilemapSprite)
    {
        var tilemapObject = _grid.GetGridObject(worldPosition);
        tilemapObject?.SetTilemapSprite(tilemapSprite);
    }

    public void SetTilemapVisual(TilemapVisual tilemapVisual)
    {
        tilemapVisual.SetGrid(this, _grid);
    }

    public void Save()
    {
        List<TilemapObject.SaveObject> tilemapObjectSaveObjectList = new List<TilemapObject.SaveObject>();
        for (int x = 0; x < _grid.Width; x++)
        {
            for (int y = 0; y < _grid.Height; y++)
            {
                TilemapObject tilemapObject = _grid.GetGridObject(x, y);
                tilemapObjectSaveObjectList.Add(tilemapObject.GetSaveObject());
            }
        }

        SaveObject saveObject = new SaveObject() {TilemapObjectSaveObjectArray = tilemapObjectSaveObjectList.ToArray()};

        SaveSystem.SaveObject(saveObject);
    }

    public void Load()
    {
        SaveObject saveObject = SaveSystem.LoadMostRecentObject<SaveObject>();
        foreach (TilemapObject.SaveObject tilemapObjectSaveObject in saveObject.TilemapObjectSaveObjectArray)
        {
            TilemapObject tilemapObject = _grid.GetGridObject(tilemapObjectSaveObject.X, tilemapObjectSaveObject.Y);
            tilemapObject.Load(tilemapObjectSaveObject);
        }
        OnLoaded?.Invoke(this, EventArgs.Empty);
    }

    public class SaveObject
    {
        public TilemapObject.SaveObject[] TilemapObjectSaveObjectArray;
    }

    public class TilemapObject
    {
        public enum TilemapSprite
        {
            None,
            Ground,
            Path,
            Dirt,
            Grass
        }

        private readonly Grid<TilemapObject> _grid;
        private TilemapSprite _tilemapSprite;
        private readonly int _x;
        private readonly int _y;

        public TilemapObject(Grid<TilemapObject> grid, int x, int y)
        {
            _grid = grid;
            _x = x;
            _y = y;
        }

        public TilemapSprite GetTilemapSprite()
        {
            return _tilemapSprite;
        }

        public void SetTilemapSprite(TilemapSprite tilemapSprite)
        {
            _tilemapSprite = tilemapSprite;
            _grid.TriggerGridObjectChanged(_x, _y);
        }

        public override string ToString()
        {
            return _tilemapSprite.ToString();
        }

        [Serializable]
        public class SaveObject
        {
            public TilemapSprite TilemapSprite;
            public int X;
            public int Y;
        }

        public SaveObject GetSaveObject()
        {
            return new SaveObject()
            {
                TilemapSprite = _tilemapSprite,
                X = _x,
                Y = _y
            };
        }

        public void Load(SaveObject saveObject)
        {
            _tilemapSprite = saveObject.TilemapSprite;
        }
    }
}
