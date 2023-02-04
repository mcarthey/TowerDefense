using System.Collections;
using System.Collections.Generic;
using Assets.GridMap.Scripts;
using CodeMonkey;
using CodeMonkey.Utils;
using UnityEngine;

public class Testing_LevelEditor : MonoBehaviour
{
    [SerializeField]private TilemapVisual tilemapVisual;
    private Tilemap _tilemap;
    private Tilemap.TilemapObject.TilemapSprite _tilemapSprite;

    void Start()
    {
        _tilemap = new Tilemap(20, 10, 10f, Vector3.zero);
        _tilemap.SetTilemapVisual(tilemapVisual);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            _tilemap.SetTilemapSprite(mouseWorldPosition, _tilemapSprite);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _tilemapSprite = Tilemap.TilemapObject.TilemapSprite.None;
            CMDebug.TextPopupMouse(_tilemapSprite.ToString());
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            _tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Ground;
            CMDebug.TextPopupMouse(_tilemapSprite.ToString());
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            _tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Path;
            CMDebug.TextPopupMouse(_tilemapSprite.ToString());
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            _tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Dirt;
            CMDebug.TextPopupMouse(_tilemapSprite.ToString());
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            _tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Grass;
            CMDebug.TextPopupMouse(_tilemapSprite.ToString());
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _tilemap.Save();
            CMDebug.TextPopupMouse("Saved!");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            _tilemap.Load();
            CMDebug.TextPopupMouse("Loaded!");
        }

    }
}
