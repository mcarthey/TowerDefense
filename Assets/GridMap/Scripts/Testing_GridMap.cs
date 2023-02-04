using CodeMonkey.Utils;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace Assets.GridMap.Scripts
{
    public class Testing_GridMap : MonoBehaviour
    {
        //[SerializeField] private HeatMapBoolVisual _heatMapBoolVisual;
        [SerializeField] private HeatMapGenericVisual _heatMapGenericVisual;
        private Grid<HeatMapGridObject> _grid;
        //private Grid<StringGridObject> _stringGrid;

        private void Start()
        {
            _grid = new Grid<HeatMapGridObject>(20, 10, 4f, new Vector3(150, 100), 
                (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g,x,y));
            //_stringGrid = new Grid<StringGridObject>(20, 10, 8f, Vector3.zero,
            //    (Grid<StringGridObject> g, int x, int y) => new StringGridObject(g, x, y));

            _heatMapGenericVisual.SetGrid(_grid);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // left mouse button
            {
                var position = UtilsClass.GetMouseWorldPosition();

                var heatMapGridObject = _grid.GetGridObject(position);
                heatMapGridObject?.AddValue(5);
            }

            /*
            if (Input.GetKeyDown(KeyCode.A)) { stringGrid.GetGridObject(position).AddLetter("A"); }
            if (Input.GetKeyDown(KeyCode.B)) { stringGrid.GetGridObject(position).AddLetter("B"); }
            if (Input.GetKeyDown(KeyCode.C)) { stringGrid.GetGridObject(position).AddLetter("C"); }

            if (Input.GetKeyDown(KeyCode.Alpha1)) { stringGrid.GetGridObject(position).AddNumber("1"); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { stringGrid.GetGridObject(position).AddNumber("2"); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { stringGrid.GetGridObject(position).AddNumber("3"); }
            */
        }
    }

    public class HeatMapGridObject
    {
        private const int MIN = 0;
        private const int MAX = 100;

        public int value;

        private Grid<HeatMapGridObject> _grid;
        private readonly int _x;
        private readonly int _y;

        public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
        {
            _grid = grid;
            _x = x;
            _y = y;
        }

        public void AddValue(int addValue)
        {
            value += addValue;
            value = Mathf.Clamp(value, MIN, MAX);
            _grid.TriggerGridObjectChanged(_x, _y);
        }

        public float GetValueNormalized()
        {
            return (float) value / MAX;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }

    public class StringGridObject
    {

        private Grid<StringGridObject> grid;
        private int x;
        private int y;

        private string letters;
        private string numbers;

        public StringGridObject(Grid<StringGridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            letters = "";
            numbers = "";
        }

        public void AddLetter(string letter)
        {
            letters += letter;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void AddNumber(string number)
        {
            numbers += number;
            grid.TriggerGridObjectChanged(x, y);
        }

        public override string ToString()
        {
            return letters + "\n" + numbers;
        }

    }
}
