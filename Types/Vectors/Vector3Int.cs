using System.Diagnostics.CodeAnalysis;

namespace Zion.Vectors
{
    [Serializable]
    public struct Vector3Int : IBinarySerializable<Vector3Int>, IRandomizable<Vector3Int>
    {
        public static readonly Vector3Int Zero = new Vector3Int(0);
        public static readonly Vector3Int Up = new Vector3Int(0, 1, 0);
        public static readonly Vector3Int Right = new Vector3Int(1, 0, 0);
        public static readonly Vector3Int Down = new Vector3Int(0, -1, 0);
        public static readonly Vector3Int Left = new Vector3Int(-1, 0, 0);
        public static readonly Vector3Int Forward = new Vector3Int(0, 0, 1);
        public static readonly Vector3Int Back = new Vector3Int(0, 0, -1);

        public int X, Y, Z;

        public Vector3Int Normal
        {
            get
            {
                float Magnitude = MathF.Sqrt((X * X) + (Y * Y) + (Z * Z));
                return new Vector3Int((int)(X / Magnitude), (int)(Y / Magnitude), (int)(Z / Magnitude));
            }
        }


        public Vector3Int(int Axis)
        {
            X = Axis;
            Y = Axis;
            Z = Axis;
        }
        public Vector3Int(int X, int Y, int Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public static Vector3Int operator +(Vector3Int A, Vector3Int B)
        {
            return new Vector3Int(A.X + B.X, A.Y + B.Y, A.Z + B.Z);
        }
        public static Vector3Int operator -(Vector3Int A, Vector3Int B)
        {
            return new Vector3Int(A.X - B.X, A.Y - B.Y, A.Z - B.Z);
        }
        public static Vector3Int operator *(Vector3Int A, int B)
        {
            return new Vector3Int(A.X * B, A.Y * B, A.Z * B);
        }
        public static Vector3Int operator /(Vector3Int A, int B)
        {
            return new Vector3Int(A.X / B, A.Y / B, A.Z / B);
        }

        public static Vector3Int operator --(Vector3Int Vector)
        {
            Vector.X--;
            Vector.Y--;
            Vector.Z--;
            return Vector;
        }
        public static Vector3Int operator ++(Vector3Int Vector)
        {
            Vector.X++;
            Vector.Y++;
            Vector.Z++;
            return Vector;
        }

        public static bool operator ==(Vector3Int A, Vector3Int B)
        {
            return A.X == B.X && A.Y == B.Y && A.Z == B.Z;
        }
        public static bool operator !=(Vector3Int A, Vector3Int B)
        {
            return A.X != B.X || A.Y != B.Y || A.Z != B.Z;
        }
        public static bool operator <=(Vector3Int A, Vector3Int B)
        {
            return A.X <= B.X && A.Y <= B.Y && A.Z <= B.Z;
        }
        public static bool operator >=(Vector3Int A, Vector3Int B)
        {
            return A.X >= B.X && A.Y >= B.Y && A.Y <= B.Y;
        }

        public static bool operator <(Vector3Int A, Vector3Int B)
        {
            return A.X < B.X && A.Y < B.Y && A.Z < B.Z;
        }
        public static bool operator >(Vector3Int A, Vector3Int B)
        {
            return A.X > B.X && A.Y > B.Y && A.Y < B.Y;
        }

        public override string ToString() => $"[{X}; {Y}; {Z}]";
        public override readonly bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is not null && Object is Vector3Int Vector && this == Vector;
        }
        public override readonly int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + X;
                hash = (hash * 23) + Y;
                hash = (hash * 23) + Z;
                return hash;
            }
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(X);
            Writer.Write(Y);
            Writer.Write(Z);
        }
        public static Vector3Int Read(BinaryReader Reader)
        {
            return new Vector3Int
            (
                Reader.ReadInt32(),
                Reader.ReadInt32(),
                Reader.ReadInt32()
            );
        }

        public static Vector3Int GetRandom(Random Random, Vector3Int Min, Vector3Int Max)
        {
            return new Vector3Int(Random.Next(Min.X, Max.X), Random.Next(Min.Y, Max.Y), Random.Next(Min.Z, Max.Z));
        }

        public void MoveUp() => Y++;
        public void MoveDown() => Y--;
        public void MoveRight() => X++;
        public void MoveLeft() => X--;
        public void MoveForward() => Z++;
        public void MoveBack() => Z--;

        public void Normalize() => this = Normal;


        public static double Distance(Vector3Int A, Vector3Int B)
        {
            Vector3Int Difference = A - B;

            return Math.Sqrt
            (
                (Difference.X * Difference.X) +
                (Difference.Y * Difference.Y) +
                (Difference.Z * Difference.Z)
            );
        }

        public static Vector3Int Sum(params IEnumerable<Vector3Int> Vectors)
        {
            ArgumentNullException.ThrowIfNull(Vectors);

            Vector3Int Result = new Vector3Int();

            foreach (Vector3Int Vector in Vectors)
            {
                Result += Vector;
            }

            return Result;
        }


        public static Vector3Int Abs(Vector3Int Vector)
        {
            return new Vector3Int(Math.Abs(Vector.X), Math.Abs(Vector.Y), Math.Abs(Vector.Z));
        }
        public static Vector3Int Clamp(Vector3Int Value, Vector3Int Min, Vector3Int Max)
        {
            return new Vector3Int
            (
                Math.Clamp(Value.X, Min.X, Max.X),
                Math.Clamp(Value.Y, Min.Y, Max.Y),
                Math.Clamp(Value.Z, Min.Z, Max.Z)
            );
        }

        public static bool IsPositive(in Vector3Int Value)
        {
            return int.IsPositive(Value.X) && int.IsPositive(Value.Y) && int.IsPositive(Value.Z);
        }
    }
}