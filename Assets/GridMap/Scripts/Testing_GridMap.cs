using CodeMonkey.Utils;
using UnityEngine;

public class Testing_GridMap : MonoBehaviour
{
    [SerializeField] private HeatMapVisual _heatMapVisual;
    private Grid _grid;

    private void Start()
    {
        _grid = new Grid(100, 100, 4f, Vector3.zero);
        _heatMapVisual.SetGrid(_grid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left mouse button
        {
            var position = UtilsClass.GetMouseWorldPosition();

            // first "fullValueRange" areas around click will have full "value"
            // the rest will keep going down until it hits "totalRange" 
            _grid.AddValue(position, 100, 2,15); 

            //var value = _grid.GetValue(position);
            //_grid.SetValue(position, value + 5);
        }
    }
}
