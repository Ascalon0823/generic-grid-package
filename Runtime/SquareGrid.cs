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

        public static SquareDir GetDir(Vector2Int dir)
        {
            return DirCoordToEnum[dir];
        }
    }

    public class SquareGrid<T> : SpatialGrid<T, Vector2Int>
    {
        public SquareGrid(int x, int y, Vector3 anchorPos,Vector3 cellSize) : base(x, y, anchorPos,
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
            return AnchorPos + new Vector3(coord.x * CellSize.x, coord.y * CellSize.z) + HalfCellSize;
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
            return new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.z));
        }
        public override bool IsValid(Vector2Int coord)
        {
            throw new NotImplementedException();
        }
    }
}