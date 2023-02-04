using System.Collections;
using System.Collections.Generic;
using Assets.GridMap.Scripts;
using UnityEngine;

public class Pathfinding
{
    private Grid<PathNode> _grid;
    public Pathfinding(int width, int height)
    {
        _grid = new Grid<PathNode>(width, height, 10f, Vector3.zero, (g, x, y) => new PathNode(g, x, y));
    }
}
