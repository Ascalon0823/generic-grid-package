using System;
using System.Collections.Generic;
using UnityEngine;

namespace CGS.Grid
{
    public enum SquareDir
    {
        Up = 0,
        UpRight = 1,
        Right = 2,
        DownRight = 3,
        Down = 4,
        DownLeft = 5,
        Left = 6,
        UpLeft = 7
    }

    public static class SquareDirExtension
    {
        private static readonly Dictionary<Vector2Int, SquareDir> DirCoordToEnum =
            new Dictionary<Vector2Int, SquareDir>
            {
                {new Vector2Int(0, 1), SquareDir.Up},
                {new Vector2Int(1, 1), SquareDir.UpRight},
                {new Vector2Int(1, 0), SquareDir.Right},
                {new Vector2Int(1, -1), SquareDir.DownRight},
                {new Vector2Int(0, -1), SquareDir.Down},
                {new Vector2Int(-1, -1), SquareDir.DownLeft},
                {new Vector2Int(-1, 0), SquareDir.Left},
                {new Vector2Int(-1, 1), SquareDir.UpLeft},
            };
        private static readonly Dictionary<int, SquareDir> IntCoordMap =
            new Dictionary<int, SquareDir>
            {
                {-1, SquareDir.UpLeft},
                {0, SquareDir.Up},
                {1, SquareDir.UpRight},
                {2, SquareDir.Right},
                {3, SquareDir.DownRight},
                {4, SquareDir.Down},
                {5, SquareDir.DownLeft},
                {6, SquareDir.Left},
                {7, SquareDir.UpLeft},
                {8, SquareDir.Up},
            };
        public static Vector2Int ToCoord(this SquareDir squareDir)
        {
            return squareDir switch
            {
                SquareDir.Up => new Vector2Int(0, 1),
                SquareDir.UpRight => new Vector2Int(1, 1),
                SquareDir.Right => new Vector2Int(1, 0),
                SquareDir.DownRight => new Vector2Int(1, -1),
                SquareDir.Down => new Vector2Int(0, -1),
                SquareDir.DownLeft => new Vector2Int(-1, -1),
                SquareDir.Left => new Vector2Int(-1, 0),
                SquareDir.UpLeft => new Vector2Int(-1, 1),
                _ => throw new ArgumentOutOfRangeException(nameof(squareDir), squareDir, null)
            };
        }

        public static Vector3 To2DVector3(this SquareDir squareDir)
        {
            return (Vector2) squareDir.ToCoord();
        }

        public static Vector3 ToVector3(this SquareDir squareDir)
        {
            return squareDir switch
            {
                SquareDir.Up => new Vector3(0, 0, 1),
                SquareDir.UpRight => new Vector3(1, 0, 1),
                SquareDir.Right => new Vector3(1, 0, 0),
                SquareDir.DownRight => new Vector3(1, 0, -1),
                SquareDir.Down => new Vector3(0, 0, -1),
                SquareDir.DownLeft => new Vector3(-1, 0, -1),
                SquareDir.Left => new Vector3(-1, 0, 0),
                SquareDir.UpLeft => new Vector3(-1, 0, 1),
                _ => throw new ArgumentOutOfRangeException(nameof(squareDir), squareDir, null)
            };
        }

        public static SquareDir Opposite(this SquareDir squareDir)
        {
            return squareDir switch
            {
                SquareDir.Up => SquareDir.Down,
                SquareDir.UpRight => SquareDir.DownLeft,
                SquareDir.Right => SquareDir.Left,
                SquareDir.DownRight => SquareDir.UpLeft,
                SquareDir.Down => SquareDir.Up,
                SquareDir.DownLeft => SquareDir.UpRight,
                SquareDir.Left => SquareDir.Right,
                SquareDir.UpLeft => SquareDir.DownRight,
                _ => throw new ArgumentOutOfRangeException(nameof(squareDir), squareDir, null)
            };
        }

        public static SquareDir Next(this SquareDir squareDir)
        {
            return IntCoordMap[(int) squareDir + 1];
        }
        
        public static SquareDir Prev(this SquareDir squareDir)
        {
            return IntCoordMap[(int) squareDir - 1];
        }


        public static SquareDir GetDir(Vector2Int dir)
        {
            return DirCoordToEnum[dir];
        }

        public static Vector2Int[] Neighbours(this Vector2Int pos, int depth = 1)
        {
            var result = new Vector2Int[depth * 8];
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < depth; j++)
                {
                    result[i * depth + j] = pos + ((SquareDir) i).ToCoord() * depth + ((SquareDir) i).ToCoord() * j;
                }
            }

            return result;
        }


        public static Vector2Int Neighbour(this Vector2Int pos, SquareDir dir, int depth = 1)
        {
            return pos + dir.ToCoord() * depth;
        }

        public static int DistanceTo(this Vector2Int from, Vector2Int to)
        {
            return Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y - to.y);
        }
    }

    public class SquareGrid<T> : SpatialGrid<T, Vector2Int>
    {
        public SquareGrid(int x, int y, Vector3 anchorPos, Vector3 cellSize) : base(x, y, anchorPos,
            cellSize)
        {
        }

        public SquareGrid(int x, int y, T val, Vector3 anchorPos, Vector3 cellSize) : base(x, y, val, anchorPos,
            cellSize)
        {
        }

        public SquareGrid(T[,] d, Vector3 anchorPos, Vector3 cellSize) : base(d, anchorPos, cellSize)
        {
        }

        public override Vector3 GetPos(Vector2Int coord)
        {
            return AnchorPos + new Vector3(coord.x * CellSize.x, 0, coord.y * CellSize.z) + HalfCellSize;
        }

        public override T GetCell(Vector2Int coord)
        {
            return this[coord.x, coord.y];
        }

        public override T GetCell(Vector3 pos)
        {
            return GetCell(FromPos(pos));
        }

        public override Vector2Int FromPos(Vector3 pos)
        {
            return new Vector2Int(Mathf.FloorToInt(pos.x - AnchorPos.x), Mathf.FloorToInt(pos.z - AnchorPos.z));
        }

        public override bool IsValid(Vector2Int coord)
        {
            return IsValid(coord.x, coord.y);
        }
    }
}