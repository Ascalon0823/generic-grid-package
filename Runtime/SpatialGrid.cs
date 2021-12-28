using UnityEngine;

namespace CGS.Grid
{
    public abstract class SpatialGrid<T, TCoord> : Grid<T>
    {
        public Vector3 AnchorPos { get; }
        public Vector3 CellSize { get; }
        public Vector3 HalfCellSize => CellSize / 2f;

        protected SpatialGrid(int x, int y, Vector3 anchorPos, Vector3 cellSize) : base(x, y)
        {
            CellSize = cellSize;
            AnchorPos = anchorPos;
        }

        protected SpatialGrid(int x, int y, T val, Vector3 anchorPos, Vector3 cellSize) : base(x, y, val)
        {
            AnchorPos = anchorPos;
            CellSize = cellSize;
        }

        protected SpatialGrid(T[,] d, Vector3 anchorPos, Vector3 cellSize) : base(d)
        {
            AnchorPos = anchorPos;
            CellSize = cellSize;
        }

        public abstract Vector3 GetPos(TCoord coord);
        public abstract T GetCell(TCoord coord);
        public abstract T GetCell(Vector3 pos);
        public abstract TCoord FromPos(Vector3 pos);
        public abstract bool IsValid(TCoord coord);
    }
}