using UnityEngine;

namespace CGS.Grid
{
    public abstract class SpatialGrid<T, TCoord> : Grid<T>
    {
        public Vector3 AnchorPos { get; }
        public Vector3 CellSize { get; }
        public Vector3 HalfCellSize => CellSize / 2f;

        public SpatialGrid(int x, int y, Vector3 anchorPos, Vector3 cellSize) : base(x, y)
        {
            CellSize = cellSize;
            AnchorPos = anchorPos;
        }

        public SpatialGrid(int x, int y, T val, Vector3 anchorPos, Vector3 cellSize) : base(x, y, val)
        {
            AnchorPos = anchorPos;
            CellSize = cellSize;
        }

        public SpatialGrid(T[,] d, Vector3 anchorPos, Vector3 cellSize) : base(d)
        {
            AnchorPos = anchorPos;
            CellSize = cellSize;
        }

        public abstract Vector3 GetPos(TCoord coord);
        public abstract T GetCell(TCoord coord);
        public abstract T GetCell(Vector3 pos);
        public abstract TCoord FromPos(Vector3 pos);
        public abstract TCoord[] GetNeighbours(TCoord coord);
    }
}