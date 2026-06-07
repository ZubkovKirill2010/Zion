using System.Diagnostics.CodeAnalysis;

namespace Zion.Vectors
{
    [Serializable]
    public struct Vector2 : IBinarySerializable<Vector2>, IRandomizable<Vector2>
    {
        public static readonly Vector2 Zero = new Vector2(0, 0);
        public static readonly Vector2 Up = new Vector2(0, 1);
        public static readonly Vector2 Right = new Vector2(1, 0);
        public static readonly Vector2 Down = new Vector2(0, -1);
        public static readonly Vector2 Left = new Vector2(-1, 0);

        public float X, Y;

        public Vector2 Reversed => new Vector2(Y, X);
        public Vector2 Normal
        {
            get
            {
                float Magnitude = MathF.Sqrt((X * X) + (Y * Y));
                return new Vector2(X / Magnitude, Y / Magnitude);
            }
        }


        public Vector2(float Axis)
        {
            X = Axis;
            Y = Axis;
        }
        public Vector2(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public Vector2(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public Vector2(Vector2Int Vector)
        {
            X = Vector.X;
            Y = Vector.Y;
        }


        public static Vector2 operator +(Vector2 A, Direction B)
        {
            Vector2 VectorB = GetDirection(B);
            return new Vector2(A.X + VectorB.X, A.Y + VectorB.Y);
        }
        public static Vector2 operator +(Direction A, Vector2 B)
        {
            Vector2 VectorA = GetDirection(A);
            return new Vector2(VectorA.X + VectorA.X, B.Y + B.Y);
        }

        public static Vector2 operator -(Vector2 A, Direction B)
        {
            Vector2 VectorB = GetDirection(B);
            return new Vector2(A.X - VectorB.X, A.Y - VectorB.Y);
        }
        public static Vector2 operator -(Direction A, Vector2 B)
        {
            Vector2 VectorA = GetDirection(A);
            return new Vector2(VectorA.X - VectorA.X, B.Y - B.Y);
        }

        public static Vector2 operator *(Vector2 A, Direction B)
        {
            Vector2 VectorB = GetDirection(B);
            return new Vector2(A.X * VectorB.X, A.Y * VectorB.Y);
        }
        public static Vector2 operator *(Direction A, Vector2 B)
        {
            Vector2 VectorA = GetDirection(A);
            return new Vector2(VectorA.X * VectorA.X, B.Y * B.Y);
        }

        public static Vector2 operator /(Vector2 A, Direction B)
        {
            Vector2 VectorB = GetDirection(B);
            return new Vector2(A.X / VectorB.X, A.Y / VectorB.Y);
        }
        public static Vector2 operator /(Direction A, Vector2 B)
        {
            Vector2 VectorA = GetDirection(A);
            return new Vector2(VectorA.X / VectorA.X, B.Y / B.Y);
        }

        public static Vector2 operator --(Vector2 Vector)
        {
            Vector.X--;
            Vector.Y--;
            return Vector;
        }
        public static Vector2 operator ++(Vector2 Vector)
        {
            Vector.X++;
            Vector.Y++;
            return Vector;
        }


        public static Vector2 operator +(Vector2 A, Vector2 B) => new Vector2(A.X + B.X, A.Y + B.Y);
        public static Vector2 operator -(Vector2 A, Vector2 B) => new Vector2(A.X - B.X, A.Y - B.Y);
        public static Vector2 operator *(Vector2 A, float B) => new Vector2(A.X * B, A.Y * B);
        public static Vector2 operator /(Vector2 A, float B) => new Vector2(A.X / B, A.Y / B);

        public static Vector2 operator +(Vector2 A, HorizontalDirection B) => new Vector2(A.X + (int)B, A.Y);
        public static Vector2 operator -(Vector2 A, HorizontalDirection B) => new Vector2(A.X - (int)B, A.Y);

        public static Vector2 operator +(Vector2 A, VerticalDirection B) => new Vector2(A.X, A.Y + (int)B);
        public static Vector2 operator -(Vector2 A, VerticalDirection B) => new Vector2(A.X, A.Y - (int)B);

        public static bool operator ==(Vector2 A, Vector2 B) => A.X == B.X && A.Y == B.Y;
        public static bool operator !=(Vector2 A, Vector2 B) => A.X != B.X || A.Y != B.Y;

        public static bool operator ==(Vector2 A, Vector2Int B) => A.X == B.X && A.Y == B.Y;
        public static bool operator !=(Vector2 A, Vector2Int B) => A.X != B.X && A.Y != B.Y;

        public static bool operator ==(Vector2Int A, Vector2 B) => A.X == B.X && A.Y == B.Y;
        public static bool operator !=(Vector2Int A, Vector2 B) => A.X == B.X && A.Y == B.Y;

        public static bool operator <(Vector2 A, Vector2 B) => A.X < B.X && A.Y < B.Y;
        public static bool operator >(Vector2 A, Vector2 B) => A.X > B.X && A.Y > B.Y;
        public static bool operator <=(Vector2 A, Vector2 B) => A.X <= B.X && A.Y <= B.Y;
        public static bool operator >=(Vector2 A, Vector2 B) => A.X >= B.X && A.Y >= B.Y;

        public static bool operator <(Vector2 A, Vector2Int B) => A.X < B.X && A.Y < B.Y;
        public static bool operator >(Vector2 A, Vector2Int B) => A.X > B.X && A.Y > B.Y;
        public static bool operator <=(Vector2 A, Vector2Int B) => A.X <= B.X && A.Y <= B.Y;
        public static bool operator >=(Vector2 A, Vector2Int B) => A.X >= B.X && A.Y >= B.Y;

        public static bool operator <(Vector2Int A, Vector2 B) => A.X < B.X && A.Y < B.Y;
        public static bool operator >(Vector2Int A, Vector2 B) => A.X > B.X && A.Y > B.Y;
        public static bool operator <=(Vector2Int A, Vector2 B) => A.X <= B.X && A.Y <= B.Y;
        public static bool operator >=(Vector2Int A, Vector2 B) => A.X >= B.X && A.Y >= B.Y;

        public static explicit operator Vector2Int(Vector2 Vector) => new Vector2Int((int)Vector.X, (int)Vector.Y);


        public override readonly string ToString() => $"[{X}; {Y}]";
        public override readonly bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is not null && Object is Vector2 Vector && this == Vector;
        }
        public override readonly int GetHashCode()
        {
            unchecked
            {
                int Hash = 17;
                Hash = (Hash * 23) + X.GetHashCode();
                Hash = (Hash * 23) + Y.GetHashCode();
                return Hash;
            }
        }


        public void MoveUp() => Y++;
        public void MoveDown() => Y--;
        public void MoveRight() => X++;
        public void MoveLeft() => X--;

        public void Reverse() => this = Reversed;
        public void Normalize(Vector2 Vector) => this = Normal;


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(X);
            Writer.Write(Y);
        }
        public static Vector2 Read(BinaryReader Reader)
        {
            return new Vector2
            (
                Reader.ReadSingle(),
                Reader.ReadSingle()
            );
        }

        public static Vector2 GetRandom(Random Random, Vector2 Min, Vector2 Max)
        {
            return new Vector2(Random.NextFloat(Min.X, Max.X), Random.NextFloat(Min.Y, Max.Y));
        }


        public static Vector2 GetDirection(Direction Direction)
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
        public static Vector2 GetDirection(int Index)
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


        public static double Distance(Vector2 A, Vector2 B)
        {
            Vector2 Difference = A - B;

            return Math.Sqrt
            (
                (Difference.X * Difference.X) +
                (Difference.Y * Difference.Y)
            );
        }

        public static bool CompareDistance(Vector2 A, Vector2 B, float MaxDistance)
        {
            float DiferenceX = A.X - B.X;
            float DiferenceY = A.Y - B.Y;
            return (DiferenceX * DiferenceX) + (DiferenceY * DiferenceY) <= MaxDistance * MaxDistance;
        }

        public static Vector2 Lerp(Vector2 A, Vector2 B, float Alpha)
        {
            return A + ((B - A) * Alpha);
        }

        public static Vector2 Sum(params IEnumerable<Vector2> Vectors)
        {
            ArgumentNullException.ThrowIfNull(Vectors);

            Vector2 Result = new Vector2();

            foreach (Vector2 Vector in Vectors)
            {
                Result += Vector;
            }

            return Result;
        }


        public static Vector2 Abs(Vector2 Vector)
        {
            return new Vector2(Math.Abs(Vector.X), Math.Abs(Vector.Y));
        }
        public static Vector2 Clamp(Vector2 Value, Vector2 Min, Vector2 Max)
        {
            return new Vector2
            (
                Math.Clamp(Value.X, Min.X, Max.X),
                Math.Clamp(Value.Y, Min.Y, Max.Y)
            );
        }
    }
}