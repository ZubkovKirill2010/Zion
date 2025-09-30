using System.Diagnostics.CodeAnalysis;

namespace Zion.Vectors
{
    [Serializable]
    public struct Vector2Int : IBinaryObject<Vector2Int>, IRandomizable<Vector2Int>
    {
        public static readonly Vector2Int Zero = new Vector2Int(0, 0);
        public static readonly Vector2Int Up = new Vector2Int(0, 1);
        public static readonly Vector2Int Right = new Vector2Int(1, 0);
        public static readonly Vector2Int Down = new Vector2Int(0, -1);
        public static readonly Vector2Int Left = new Vector2Int(-1, 0);
        public static readonly Vector2Int OneOne = new Vector2Int(1);

        public int x, y;

        public Vector2Int Reversed => new Vector2Int(y, x);
        public Vector2Int Normal
        {
            get
            {
                float Magnitude = MathF.Sqrt(x * x + y * y);
                return new Vector2Int((int)(x / Magnitude), (int)(y / Magnitude));
            }
        }


        public Vector2Int(int Axis)
        {
            x = Axis;
            y = Axis;
        }
        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }


        public static Vector2Int operator +(Vector2Int A, Direction B)
        {
            Vector2Int VectorB = GetDirection(B);
            return new Vector2Int(A.x + VectorB.x, A.y + VectorB.y);
        }
        public static Vector2Int operator -(Vector2Int A, Direction B)
        {
            Vector2Int VectorB = GetDirection(B);
            return new Vector2Int(A.x - VectorB.x, A.y - VectorB.y);
        }
        public static Vector2Int operator *(Vector2Int A, Direction B)
        {
            Vector2Int VectorB = GetDirection(B);
            return new Vector2Int(A.x * VectorB.x, A.y * VectorB.y);
        }
        public static Vector2Int operator /(Vector2Int A, Direction B)
        {
            Vector2Int VectorB = GetDirection(B);
            return new Vector2Int(A.x / VectorB.x, A.y / VectorB.y);
        }

        public static Vector2Int operator --(Vector2Int Vector)
        {
            Vector.x--;
            Vector.y--;
            return Vector;
        }
        public static Vector2Int operator ++(Vector2Int Vector)
        {
            Vector.x++;
            Vector.y++;
            return Vector;
        }

        public static Vector2Int operator +(Vector2Int A, Vector2Int B) => new Vector2Int(A.x + B.x, A.y + B.y);
        public static Vector2Int operator -(Vector2Int A, Vector2Int B) => new Vector2Int(A.x - B.x, A.y - B.y);
        public static Vector2Int operator *(Vector2Int A, int B) => new Vector2Int(A.x * B, A.y * B);
        public static Vector2Int operator /(Vector2Int A, int B) => new Vector2Int(A.x / B, A.y / B);

        public static Vector2Int operator +(Vector2Int A, Vector2Byte B) => new Vector2Int(A.x + B.x, A.y + B.y);
        public static Vector2Int operator -(Vector2Int A, Vector2Byte B) => new Vector2Int(A.x - B.x, A.y - B.y);

        public static Vector2Int operator +(Vector2Int A, HorizontalDirection B) => new Vector2Int(A.x + (int)B, A.y);
        public static Vector2Int operator -(Vector2Int A, HorizontalDirection B) => new Vector2Int(A.x - (int)B, A.y);

        public static Vector2Int operator +(Vector2Int A, VerticalDirection B) => new Vector2Int(A.x, A.y + (int)B);
        public static Vector2Int operator -(Vector2Int A, VerticalDirection B) => new Vector2Int(A.x, A.y - (int)B);

        public static bool operator ==(Vector2Int A, Vector2Int B) => A.x == B.x && A.y == B.y;
        public static bool operator !=(Vector2Int A, Vector2Int B) => A.x != B.x || A.y != B.y;

        public static bool operator <(Vector2Int A, Vector2Int B) => A.x < B.x && A.y < B.y;
        public static bool operator >(Vector2Int A, Vector2Int B) => A.x > B.x && A.y > B.y;
        public static bool operator <=(Vector2Int A, Vector2Int B) => A.x <= B.x && A.y <= B.y;
        public static bool operator >=(Vector2Int A, Vector2Int B) => A.x >= B.x && A.y >= B.y;

        public static explicit operator Vector2(Vector2Int Vector) => new Vector2(Vector.x, Vector.y);


        public override readonly string ToString() => $"[{x}, {y}]";
        public override readonly bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is not null && Object is Vector2Int Vector && this == Vector;
        }
        public override readonly int GetHashCode()
        {
            unchecked
            {
                int Hash = 17;
                Hash = Hash * 23 + x;
                Hash = Hash * 23 + y;
                return Hash;
            }
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(x);
            Writer.Write(y);
        }
        public static Vector2Int Read(BinaryReader Reader)
        {
            return new Vector2Int
            (
                Reader.ReadInt32(),
                Reader.ReadInt32()
            );
        }

        public static Vector2Int GetRandom(Random Random, Vector2Int Min, Vector2Int Max)
        {
            return new Vector2Int(Random.Next(Min.x, Max.x), Random.Next(Min.y, Max.y));
        }


        public void MoveUp() => y++;
        public void MoveDown() => y--;
        public void MoveRight() => x++;
        public void MoveLeft() => x--;

        public void Reverse() => Accessor.Reverse(ref x, ref y);


        public static Vector2Int GetRandomDirection()
        {
            return GetRandomDirection(new Random());
        }
        public static Vector2Int GetRandomDirection(Random Random)
        {
            return GetDirection(Random.Next(4));
        }

        public static Vector2Int[] GetRandomDirections()
        {
            return GetRandomDirections(new Random());
        }
        public static Vector2Int[] GetRandomDirections(Random Random)
        {
            return new List<Vector2Int>
            {
                Up,
                Right,
                Down,
                Left
            }.OrderBy(x => Random.Next()).ToArray();
        }


        public static Vector2Int GetDirection(Direction Direction)
        {
            switch (Direction)
            {
                case Direction.None: return Zero;
                case Direction.Up: return Up;
                case Direction.Down: return Down;
                case Direction.Left: return Left;
                case Direction.Right: return Right;
            }
            throw new Exception();
        }
        public static Vector2Int GetDirection(int Index)
        {
            switch (Index % 4)
            {
                case 0: return Up;
                case 1: return Down;
                case 2: return Left;
                case 3: return Right;
            }
            return Zero;
        }

        public static Direction ToDirection(Vector2Int Vector)
        {
            if (Vector == Up) { return Direction.Up; }
            if (Vector == Right) { return Direction.Right; }
            if (Vector == Down) { return Direction.Down; }
            if (Vector == Left) { return Direction.Left; }
            return Direction.None;
        }


        public static double Distance(Vector2Int A, Vector2Int B)
        {
            Vector2Int Difference = A - B;

            return Math.Sqrt
            (
                (Difference.x * Difference.x) +
                (Difference.y * Difference.y)
            );
        }

        public static Vector2Int Sum(params IEnumerable<Vector2Int> Vectors)
        {
            ArgumentNullException.ThrowIfNull(Vectors);

            Vector2Int Result = new Vector2Int();

            foreach (Vector2Int Vector in Vectors)
            {
                Result += Vector;
            }

            return Result;
        }


        public static Vector2Int Absolute(Vector2Int Vector)
        {
            return new Vector2Int(Math.Abs(Vector.x), Math.Abs(Vector.y));
        }
        public static Vector2Int Clamp(Vector2Int Value, Vector2Int Min, Vector2Int Max)
        {
            return new Vector2Int
            (
                Math.Clamp(Value.x, Min.x, Max.x),
                Math.Clamp(Value.y, Min.y, Max.y)
            );
        }
    }
}