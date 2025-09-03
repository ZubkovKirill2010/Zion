using System.Diagnostics.CodeAnalysis;

namespace Zion.Vectors
{
    [Serializable]
    public struct Vector2 : IBinaryObject<Vector2>
    {
        public static readonly Vector2 Zero = new Vector2(0, 0);
        public static readonly Vector2 Up = new Vector2(0, 1);
        public static readonly Vector2 Right = new Vector2(1, 0);
        public static readonly Vector2 Down = new Vector2(0, -1);
        public static readonly Vector2 Left = new Vector2(-1, 0);

        public float x, y;

        public Vector2 Reversed => new Vector2(y, x);
        public Vector2 Normal
        {
            get
            {
                float Magnitude = MathF.Sqrt(x * x + y * y);
                return new Vector2(x / Magnitude, y / Magnitude);
            }
        }


        public Vector2(float Axis)
        {
            x = Axis;
            y = Axis;
        }
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2(Vector2Int Vector)
        {
            x = Vector.x;
            y = Vector.y;
        }


        public static Vector2 operator +(Vector2 A, Direction B)
        {
            Vector2 VectorB = GetDirection(B);
            return new Vector2(A.x + VectorB.x, A.y + VectorB.y);
        }
        public static Vector2 operator +(Direction A, Vector2 B)
        {
            Vector2 VectorA = GetDirection(A);
            return new Vector2(VectorA.x + VectorA.x, B.y + B.y);
        }

        public static Vector2 operator -(Vector2 A, Direction B)
        {
            Vector2 VectorB = GetDirection(B);
            return new Vector2(A.x - VectorB.x, A.y - VectorB.y);
        }
        public static Vector2 operator -(Direction A, Vector2 B)
        {
            Vector2 VectorA = GetDirection(A);
            return new Vector2(VectorA.x - VectorA.x, B.y - B.y);
        }

        public static Vector2 operator *(Vector2 A, Direction B)
        {
            Vector2 VectorB = GetDirection(B);
            return new Vector2(A.x * VectorB.x, A.y * VectorB.y);
        }
        public static Vector2 operator *(Direction A, Vector2 B)
        {
            Vector2 VectorA = GetDirection(A);
            return new Vector2(VectorA.x * VectorA.x, B.y * B.y);
        }

        public static Vector2 operator /(Vector2 A, Direction B)
        {
            Vector2 VectorB = GetDirection(B);
            return new Vector2(A.x / VectorB.x, A.y / VectorB.y);
        }
        public static Vector2 operator /(Direction A, Vector2 B)
        {
            Vector2 VectorA = GetDirection(A);
            return new Vector2(VectorA.x / VectorA.x, B.y / B.y);
        }

        public static Vector2 operator --(Vector2 Vector)
        {
            Vector.x--;
            Vector.y--;
            return Vector;
        }
        public static Vector2 operator ++(Vector2 Vector)
        {
            Vector.x++;
            Vector.y++;
            return Vector;
        }


        public static Vector2 operator +(Vector2 A, Vector2 B) => new Vector2(A.x + B.x, A.y + B.y);
        public static Vector2 operator -(Vector2 A, Vector2 B) => new Vector2(A.x - B.x, A.y - B.y);
        public static Vector2 operator *(Vector2 A, float B) => new Vector2(A.x * B, A.y * B);
        public static Vector2 operator /(Vector2 A, float B) => new Vector2(A.x / B, A.y / B);

        public static bool operator ==(Vector2 A, Vector2 B) => A.x == B.x && A.y == B.y;
        public static bool operator !=(Vector2 A, Vector2 B) => A.x != B.x || A.y != B.y;

        public static bool operator ==(Vector2 A, Vector2Int B) => A.x == B.x && A.y == B.y;
        public static bool operator !=(Vector2 A, Vector2Int B) => A.x != B.x && A.y != B.y;

        public static bool operator ==(Vector2Int A, Vector2 B) => A.x == B.x && A.y == B.y;
        public static bool operator !=(Vector2Int A, Vector2 B) => A.x == B.x && A.y == B.y;

        public static bool operator <(Vector2 A, Vector2 B) => A.x < B.x && A.y < B.y;
        public static bool operator >(Vector2 A, Vector2 B) => A.x > B.x && A.y > B.y;
        public static bool operator <=(Vector2 A, Vector2 B) => A.x <= B.x && A.y <= B.y;
        public static bool operator >=(Vector2 A, Vector2 B) => A.x >= B.x && A.y >= B.y;

        public static bool operator <(Vector2 A, Vector2Int B) => A.x < B.x && A.y < B.y;
        public static bool operator >(Vector2 A, Vector2Int B) => A.x > B.x && A.y > B.y;
        public static bool operator <=(Vector2 A, Vector2Int B) => A.x <= B.x && A.y <= B.y;
        public static bool operator >=(Vector2 A, Vector2Int B) => A.x >= B.x && A.y >= B.y;

        public static bool operator <(Vector2Int A, Vector2 B) => A.x < B.x && A.y < B.y;
        public static bool operator >(Vector2Int A, Vector2 B) => A.x > B.x && A.y > B.y;
        public static bool operator <=(Vector2Int A, Vector2 B) => A.x <= B.x && A.y <= B.y;
        public static bool operator >=(Vector2Int A, Vector2 B) => A.x >= B.x && A.y >= B.y;

        public static explicit operator Vector2Int(Vector2 Vector) => new Vector2Int((int)Vector.x, (int)Vector.y);


        public override readonly string ToString() => $"[{x}, {y}]";
        public override readonly bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is not null && Object is Vector2 Vector && this == Vector;
        }
        public override readonly int GetHashCode()
        {
            unchecked
            {
                int Hash = 17;
                Hash = Hash * 23 + x.GetHashCode();
                Hash = Hash * 23 + y.GetHashCode();
                return Hash;
            }
        }


        public void MoveUp() => y++;
        public void MoveDown() => y--;
        public void MoveRight() => x++;
        public void MoveLeft() => x--;

        public void Reverse() => this = Reversed;
        public void Normalize(Vector2 Vector) => this = Normal;


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(x);
            Writer.Write(y);
        }
        public static Vector2 Read(BinaryReader Reader)
        {
            return new Vector2
            (
                Reader.ReadSingle(),
                Reader.ReadSingle()
            );
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
                (Difference.x * Difference.x) +
                (Difference.y * Difference.y)
            );
        }

        public static Vector2 Lerp(Vector2 A, Vector2 B, float Alpha)
        {
            return A + (B - A) * Alpha;
        }


        public static Vector2 Absolute(Vector2 Vector)
        {
            return new Vector2(Math.Abs(Vector.x), Math.Abs(Vector.y));
        }
        public static Vector2 Clamp(Vector2 Value, Vector2 Min, Vector2 Max)
        {
            return new Vector2
            (
                Math.Clamp(Value.x, Min.x, Max.x),
                Math.Clamp(Value.y, Min.y, Max.y)
            );
        }
    }
}