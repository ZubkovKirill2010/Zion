using System.Diagnostics.CodeAnalysis;

namespace Zion.Vectors
{
    [Serializable]
    public struct Vector2Int : IBinarySerializable<Vector2Int>, IRandomizable<Vector2Int>
    {
        public static readonly Vector2Int Zero = new Vector2Int(0, 0);
        public static readonly Vector2Int Up = new Vector2Int(0, 1);
        public static readonly Vector2Int Right = new Vector2Int(1, 0);
        public static readonly Vector2Int Down = new Vector2Int(0, -1);
        public static readonly Vector2Int Left = new Vector2Int(-1, 0);
        public static readonly Vector2Int OneOne = new Vector2Int(1);

        public int X, Y;

        public Vector2Int Reversed => new Vector2Int(Y, X);
        public Vector2Int Normal
        {
            get
            {
                float Magnitude = MathF.Sqrt((X * X) + (Y * Y));
                return new Vector2Int((int)(X / Magnitude), (int)(Y / Magnitude));
            }
        }


        public Vector2Int(int Axis)
        {
            X = Axis;
            Y = Axis;
        }
        public Vector2Int(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }


        public static Vector2Int operator +(Vector2Int A, Direction B)
        {
            Vector2Int VectorB = GetDirection(B);
            return new Vector2Int(A.X + VectorB.X, A.Y + VectorB.Y);
        }
        public static Vector2Int operator -(Vector2Int A, Direction B)
        {
            Vector2Int VectorB = GetDirection(B);
            return new Vector2Int(A.X - VectorB.X, A.Y - VectorB.Y);
        }
        public static Vector2Int operator *(Vector2Int A, Direction B)
        {
            Vector2Int VectorB = GetDirection(B);
            return new Vector2Int(A.X * VectorB.X, A.Y * VectorB.Y);
        }
        public static Vector2Int operator /(Vector2Int A, Direction B)
        {
            Vector2Int VectorB = GetDirection(B);
            return new Vector2Int(A.X / VectorB.X, A.Y / VectorB.Y);
        }

        public static Vector2Int operator --(Vector2Int Vector)
        {
            Vector.X--;
            Vector.Y--;
            return Vector;
        }
        public static Vector2Int operator ++(Vector2Int Vector)
        {
            Vector.X++;
            Vector.Y++;
            return Vector;
        }

        public static Vector2Int operator +(Vector2Int A, Vector2Int B) => new Vector2Int(A.X + B.X, A.Y + B.Y);
        public static Vector2Int operator -(Vector2Int A, Vector2Int B) => new Vector2Int(A.X - B.X, A.Y - B.Y);
        public static Vector2Int operator *(Vector2Int A, int B) => new Vector2Int(A.X * B, A.Y * B);
        public static Vector2Int operator /(Vector2Int A, int B) => new Vector2Int(A.X / B, A.Y / B);

        public static Vector2Int operator &(Vector2Int A, Vector2Int B) => new Vector2Int(A.X & B.X, A.Y & B.Y);
        public static Vector2Int operator |(Vector2Int A, Vector2Int B) => new Vector2Int(A.X & B.X, A.Y & B.Y);

        public static Vector2Int operator &(Vector2Int A, int B) => new Vector2Int(A.X & B, A.Y & B);
        public static Vector2Int operator |(Vector2Int A, int B) => new Vector2Int(A.X & B, A.Y & B);

        public static Vector2Int operator >>(Vector2Int A, int B) => new Vector2Int(A.X >> B, A.Y >> B);
        public static Vector2Int operator <<(Vector2Int A, int B) => new Vector2Int(A.X << B, A.Y << B);

        public static Vector2Int operator +(Vector2Int A, Vector2Byte B) => new Vector2Int(A.X + B.X, A.Y + B.Y);
        public static Vector2Int operator -(Vector2Int A, Vector2Byte B) => new Vector2Int(A.X - B.X, A.Y - B.Y);

        public static Vector2Int operator +(Vector2Int A, HorizontalDirection B) => new Vector2Int(A.X + (int)B, A.Y);
        public static Vector2Int operator -(Vector2Int A, HorizontalDirection B) => new Vector2Int(A.X - (int)B, A.Y);

        public static Vector2Int operator +(Vector2Int A, VerticalDirection B) => new Vector2Int(A.X, A.Y + (int)B);
        public static Vector2Int operator -(Vector2Int A, VerticalDirection B) => new Vector2Int(A.X, A.Y - (int)B);

        public static bool operator ==(Vector2Int A, Vector2Int B) => A.X == B.X && A.Y == B.Y;
        public static bool operator !=(Vector2Int A, Vector2Int B) => A.X != B.X || A.Y != B.Y;

        public static bool operator <(Vector2Int A, Vector2Int B) => A.X < B.X && A.Y < B.Y;
        public static bool operator >(Vector2Int A, Vector2Int B) => A.X > B.X && A.Y > B.Y;
        public static bool operator <=(Vector2Int A, Vector2Int B) => A.X <= B.X && A.Y <= B.Y;
        public static bool operator >=(Vector2Int A, Vector2Int B) => A.X >= B.X && A.Y >= B.Y;

        public static explicit operator Vector2(Vector2Int Vector) => new Vector2(Vector.X, Vector.Y);


        public override readonly string ToString() => $"[{X}; {Y}]";
        public override readonly bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is not null && Object is Vector2Int Vector && this == Vector;
        }
        public override readonly int GetHashCode()
        {
            unchecked
            {
                int Hash = 17;
                Hash = (Hash * 23) + X;
                Hash = (Hash * 23) + Y;
                return Hash;
            }
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(X);
            Writer.Write(Y);
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
            return new Vector2Int(Random.Next(Min.X, Max.X), Random.Next(Min.Y, Max.Y));
        }


        public void MoveUp() => Y++;
        public void MoveDown() => Y--;
        public void MoveRight() => X++;
        public void MoveLeft() => X--;

        public void Reverse() => Accessor.Reverse(ref X, ref Y);


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
            }.OrderBy(X => Random.Next()).ToArray();
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
            return Vector == Up
                ? Direction.Up
                : Vector == Right ? Direction.Right : Vector == Down ? Direction.Down : Vector == Left ? Direction.Left : Direction.None;
        }


        public static double Distance(Vector2Int A, Vector2Int B)
        {
            Vector2Int Difference = A - B;

            return Math.Sqrt
            (
                (Difference.X * Difference.X) +
                (Difference.Y * Difference.Y)
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


        public static bool IsNegative(in Vector2Int Value)
        {
            return int.IsNegative(Value.X) || int.IsNegative(Value.Y);
        }

        public static bool IsPositive(in Vector2Int Value)
        {
            return int.IsPositive(Value.X) && int.IsPositive(Value.Y);
        }


        public static Vector2Int Abs(Vector2Int Vector)
        {
            return new Vector2Int(Math.Abs(Vector.X), Math.Abs(Vector.Y));
        }

        public static Vector2Int Clamp(Vector2Int Value, Vector2Int Min, Vector2Int Max)
        {
            return new Vector2Int
            (
                Math.Clamp(Value.X, Min.X, Max.X),
                Math.Clamp(Value.Y, Min.Y, Max.Y)
            );
        }
    }
}