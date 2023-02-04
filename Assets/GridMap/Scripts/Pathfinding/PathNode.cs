using System.Collections;
using System.Collections.Generic;
using Assets.GridMap.Scripts;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PathNode 
{
    private Grid<PathNode> _grid;
    private int _x;
    private int _y;

    public int GCost;
    public int HCost;
    public int FCost;

    public PathNode CameFromNode;

    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        _grid = grid;
        _x = x;
        _y = y;
    }

    public override string ToString()
    {
        return $"{_x},{_y}";
    }
}
