namespace CGS.Grid
{
    [System.Serializable]
    public class Grid<T>
    {
        public T[,] Data { get; }

        public Grid(int x, int y)
        {
            Data = new T[x, y];
        }

        public Grid(int x, int y, T val)
        {
            Data = new T[x, y];
            Fill(val);
        }

        public Grid(T[,] d)
        {
            Data = d;
        }
        

        public int Width()
        {
            return Data.GetLength(0);
        }

        public int Height()
        {
            return Data.GetLength(1);
        }

        public bool IsBoarder(int x, int y)
        {
            return x == 0 || y == 0 || x == Width() - 1 || y == Height() - 1;
        }


        public bool IsValid(int x, int y)
        {
            return Data != null && x >= 0 && y >= 0 && x < Width() && y < Height();
        }

        public T this[int x, int y]
        {
            get => Data[x, y];
            set => Data[x, y] = value;
        }
        

        public void Fill(T val)
        {
            Iterate((i, j) => Data[i, j] = val);
        }

        public void Iterate(System.Action<int, int> action)
        {
            if (Data == null)
            {
                return;
            }

            for (var j = 0; j < Data.GetLength(1); j++)
            {
                for (var i = 0; i < Data.GetLength(0); i++)
                {
                    action?.Invoke(i, j);
                }
            }
        }
        
        

        public override string ToString()
        {
            if (Data == null)
            {
                return "null";
            }

            var print = "";
            for (var j = 0; j < Data.GetLength(1); j++)
            {
                print += "[";
                for (var i = 0; i < Data.GetLength(0); i++)
                {
                    print += $" {Data[i, j].ToString()} ";
                }

                print += "]\n";
            }

            return print;
        }
    }
}