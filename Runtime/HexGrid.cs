using UnityEngine;

namespace CGS.Grid
{
    public static class HexUtil
    {
        public static float InnerRadius(float outerRadius)
        {
            return outerRadius * 0.866025404f;
        }

        public static Vector3 CellSize(float outerRadius)
        {
            return new Vector3(InnerRadius(outerRadius), 0f, outerRadius) * 2f;
        }
    }

    public struct HexCoord
    {
        public int X, Z;
        public int Y => -X - Z;

        public HexCoord(int x, int z)
        {
            X = x - z / 2;
            Z = z;
        }

        public override string ToString()
        {
            return $"{X} {Y} {Z}";
        }
    }

    public class HexGrid<T> : SpatialGrid<T, HexCoord>
    {
        public HexGrid(int x, int y, Vector3 anchorPos, float radius) : base(x, y, anchorPos,
            HexUtil.CellSize(radius))
        {
        }

        public HexGrid(int x, int y, T val, Vector3 anchorPos, float radius) : base(x, y, val, anchorPos,
            HexUtil.CellSize(radius))
        {
        }

        public HexGrid(T[,] d, Vector3 anchorPos, float radius) : base(d, anchorPos, HexUtil.CellSize(radius))
        {
        }

        public override Vector3 GetPos(HexCoord coord)
        {
            return new Vector3((coord.X + coord.Z / 2) * CellSize.x + HalfCellSize.x + coord.Z % 2 * HalfCellSize.x,
                0f,
                coord.Z * HalfCellSize.z * 1.5f + HalfCellSize.z)+AnchorPos;
        }

        public override T GetCell(HexCoord coord)
        {
            return !IsValid(coord.X + coord.Z / 2, coord.Z) ? default : this[coord.X + coord.Z / 2, coord.Z];
        }

        public override HexCoord FromPos(Vector3 pos)
        {
            pos -= AnchorPos+HalfCellSize;
            var x = pos.x / CellSize.x;
            var y = -x;
            var off = pos.z / (CellSize.z * 1.5f);
            x -= off;
            y -= off;
            var iX = Mathf.RoundToInt(x);
            var iY = Mathf.RoundToInt(y);
            var iZ = Mathf.RoundToInt(-x - y);
            if (iX + iY + iZ == 0) return new HexCoord(iX+iZ/2, iZ);
            var dX = Mathf.Abs(x - iX);
            var dY = Mathf.Abs(y - iY);
            var dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }

            return new HexCoord(iX+iZ/2, iZ);
        }

        public override T GetCell(Vector3 pos)
        {
            return GetCell(FromPos(pos));
        }

        public override HexCoord[] GetNeighbours(HexCoord coord)
        {
            throw new System.NotImplementedException();
        }
    }
}