using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGS.Grid
{
    public class SquareGrid2D<T> : SquareGrid<T>
    {
        public SquareGrid2D(int x, int y, Vector3 anchorPos, Vector3 cellSize) : base(x, y, anchorPos, cellSize)
        {
        }

        public SquareGrid2D(int x, int y, T val, Vector3 anchorPos, Vector3 cellSize) : base(x, y, val, anchorPos,
            cellSize)
        {
        }

        public SquareGrid2D(T[,] d, Vector3 anchorPos, Vector3 cellSize) : base(d, anchorPos, cellSize)
        {
        }

        public override Vector2Int FromPos(Vector3 pos)
        {
            return new Vector2Int(Mathf.FloorToInt(pos.x - AnchorPos.x), Mathf.FloorToInt(pos.y - AnchorPos.y));
        }

        public override Vector3 GetPos(Vector2Int coord)
        {
            return AnchorPos + new Vector3(coord.x * CellSize.x, coord.y * CellSize.y, 0f) + HalfCellSize;
        }

        public Vector3 GetPos(int x, int y)
        {
            return AnchorPos + new Vector3(x * CellSize.x, y * CellSize.y, 0f) + HalfCellSize;
        }
    }
}