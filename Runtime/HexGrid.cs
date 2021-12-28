using System;
using System.Collections.Generic;
using UnityEngine;

namespace CGS.Grid
{
    #region Def&Utils

    /*
     *   NW NE
     * W       E
     *   SW SE
     */
    public enum HexDir
    {
        NE,
        E,
        SE,
        SW,
        W,
        NW
    }

    public static class HexDirExtensions
    {
        private static readonly HexDir[] _each = {HexDir.NE, HexDir.E, HexDir.SE, HexDir.SW, HexDir.W, HexDir.NW};

        private static readonly Dictionary<int, HexDir> _indexingMap = new Dictionary<int, HexDir>
        {
            {-1, HexDir.NW}, {0, HexDir.NE}, {1, HexDir.E},
            {2, HexDir.SE}, {3, HexDir.SW}, {4, HexDir.W},
            {5, HexDir.NW}, {6, HexDir.NE}, {7, HexDir.E},
            {8, HexDir.SE},
        };

        private static readonly Dictionary<HexDir, HexCoord> _coordMap = new Dictionary<HexDir, HexCoord>
        {
            {HexDir.NE, new HexCoord(0, 1)}, {HexDir.E, new HexCoord(1, 0)},
            {HexDir.SE, new HexCoord(1, -1)}, {HexDir.SW, new HexCoord(0, -1)},
            {HexDir.W, new HexCoord(-1, 0)}, {HexDir.NW, new HexCoord(-1, 1)}
        };

        public static HexDir[] Each()
        {
            return _each;
        }

        public static HexCoord ToCoord(this HexDir direction)
        {
            return _coordMap[direction];
        }

        public static HexDir Opposite(this HexDir direction)
        {
            return _indexingMap[(int) direction + 3];
        }

        public static HexDir Previous(this HexDir direction)
        {
            return _indexingMap[(int) direction - 1];
        }

        public static HexDir Next(this HexDir direction)
        {
            return _indexingMap[(int) direction + 1];
        }
    }

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
            X = x;
            Z = z;
        }

        public static HexCoord FromSquareCoord(int x, int z)
        {
            return new HexCoord(x - z / 2, z);
        }

        public static HexCoord operator +(HexCoord a) => a;
        public static HexCoord operator -(HexCoord a) => new HexCoord(-a.X, -a.Z);

        public static HexCoord operator +(HexCoord a, HexCoord b)
            => new HexCoord(a.X + b.X, a.Z + b.Z);

        public static HexCoord operator -(HexCoord a, HexCoord b)
            => a + (-b);

        public override string ToString()
        {
            return $"{X} {Y} {Z}";
        }

        public HexCoord[] Neighbours()
        {
            var result = new HexCoord[6];
            foreach (var dir in HexDirExtensions.Each())
            {
                result[(int) dir] = this + dir.ToCoord();
            }

            return result;
        }

        public HexCoord Neighbour(HexDir dir)
        {
            return this + dir.ToCoord();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Z);
        }

        public override bool Equals(object obj)
        {
            return obj is HexCoord other && (
                X == other.X && Y == other.Y);
        }
    }

    #endregion


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
                coord.Z * HalfCellSize.z * 1.5f + HalfCellSize.z) + AnchorPos;
        }
        public override HexCoord FromPos(Vector3 pos)
        {
            pos -= AnchorPos + HalfCellSize;
            var x = pos.x / CellSize.x;
            var y = -x;
            var off = pos.z / (CellSize.z * 1.5f);
            x -= off;
            y -= off;
            var iX = Mathf.RoundToInt(x);
            var iY = Mathf.RoundToInt(y);
            var iZ = Mathf.RoundToInt(-x - y);
            if (iX + iY + iZ == 0) return new HexCoord(iX, iZ);
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

            return new HexCoord(iX, iZ);
        }
        public override T GetCell(HexCoord coord)
        {
            return !IsValid(coord.X + coord.Z / 2, coord.Z) ? default : this[coord.X + coord.Z / 2, coord.Z];
        }
        public override T GetCell(Vector3 pos)
        {
            return GetCell(FromPos(pos));
        }

        public override bool IsValid(HexCoord coord)
        {
            return base.IsValid(coord.X + coord.Z / 2, coord.Z);
        }
    }
}