using System;
using UnityEngine;

namespace CGS.Grid.Sample
{
    public class SampleHexMap : MonoBehaviour
    {
        public class HexMap : HexGrid<string>
        {
            public HexMap(int x, int y, Vector3 anchorPos, float radius) : base(x, y, anchorPos, radius)
            {
            }

            public HexMap(int x, int y, string val, Vector3 anchorPos, float radius) : base(x, y, val, anchorPos,
                radius)
            {
            }

            public HexMap(string[,] d, Vector3 anchorPos, float radius) : base(d, anchorPos, radius)
            {
            }
        }

        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private float radius;
        [SerializeField] private TextMesh cellPrefab;
        [SerializeField] private GameObject hovererPrefab;
        [SerializeField] private string currenCell;
        private GameObject _hoverer;
        private HexMap map;

        private void Start()
        {
            if (width * height * radius == 0)
            {
                return;
            }

            map = new HexMap(width, height, transform.position, radius);
            map.Iterate((x, y) => { map[x, y] = new HexCoord(x, y).ToString(); });
            if (cellPrefab == null)
            {
                return;
            }

            map.Iterate((x, y) =>
            {
                var cell = Instantiate(cellPrefab,transform);
                cell.transform.position = map.GetPos(new HexCoord(x, y));
                cell.text = map[x, y];
                cell.gameObject.transform.localScale = new Vector3(map.CellSize.z, 1f, map.CellSize.z)*0.95f;
            });
            Camera.main.transform.position = map.GetPos(new HexCoord(width/2,height/2))+Vector3.up*10f;
        }

        private void Update()
        {
            if (!_hoverer)
            {
                if (!hovererPrefab) return;
                _hoverer = Instantiate(hovererPrefab);
            }
            var p = new Plane(Vector3.up,Vector3.zero);
            if (!Camera.current) return;
            var ray = Camera.current.ScreenPointToRay(Input.mousePosition);
            p.Raycast(ray, out var dis);
            var pos = ray.GetPoint(dis);
            currenCell = map.GetCell(pos);
            var coord = map.FromPos(pos);
            _hoverer.transform.position = map.GetPos(coord);
        }
    }
}