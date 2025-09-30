using System.Diagnostics.CodeAnalysis;

namespace Zion.Vectors
{
    [Serializable]
    public struct Vector3Int : IBinaryObject<Vector3Int>, IRandomizable<Vector3Int>
    {
        public static readonly Vector3Int Zero = new Vector3Int(0);
        public static readonly Vector3Int Up = new Vector3Int(0, 1, 0);
        public static readonly Vector3Int Right = new Vector3Int(1, 0, 0);
        public static readonly Vector3Int Down = new Vector3Int(0, -1, 0);
        public static readonly Vector3Int Left = new Vector3Int(-1, 0, 0);
        public static readonly Vector3Int Forward = new Vector3Int(0, 0, 1);
        public static readonly Vector3Int Back = new Vector3Int(0, 0, -1);

        public int x, y, z;

        public Vector3Int Normal
        {
            get
            {
                float Magnitude = MathF.Sqrt(x * x + y * y + z * z);
                return new Vector3Int((int)(x / Magnitude), (int)(y / Magnitude), (int)(z / Magnitude));
            }
        }


        public Vector3Int(int Axis)
        {
            x = Axis;
            y = Axis;
            z = Axis;
        }
        public Vector3Int(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3Int operator +(Vector3Int A, Vector3Int B)
        {
            return new Vector3Int(A.x + B.x, A.y + B.y, A.z + B.z);
        }
        public static Vector3Int operator -(Vector3Int A, Vector3Int B)
        {
            return new Vector3Int(A.x - B.x, A.y - B.y, A.z - B.z);
        }
        public static Vector3Int operator *(Vector3Int A, int B)
        {
            return new Vector3Int(A.x * B, A.y * B, A.z * B);
        }
        public static Vector3Int operator /(Vector3Int A, int B)
        {
            return new Vector3Int(A.x / B, A.y / B, A.z / B);
        }

        public static Vector3Int operator --(Vector3Int Vector)
        {
            Vector.x--;
            Vector.y--;
            Vector.z--;
            return Vector;
        }
        public static Vector3Int operator ++(Vector3Int Vector)
        {
            Vector.x++;
            Vector.y++;
            Vector.z++;
            return Vector;
        }

        public static bool operator ==(Vector3Int A, Vector3Int B)
        {
            return A.x == B.x && A.y == B.y && A.z == B.z;
        }
        public static bool operator !=(Vector3Int A, Vector3Int B)
        {
            return A.x != B.x || A.y != B.y || A.z != B.z;
        }
        public static bool operator <=(Vector3Int A, Vector3Int B)
        {
            return A.x <= B.x && A.y <= B.y && A.z <= B.z;
        }
        public static bool operator >=(Vector3Int A, Vector3Int B)
        {
            return A.x >= B.x && A.y >= B.y && A.y <= B.y;
        }

        public override string ToString() => $"[{x}, {y}, {z}]";
        public override readonly bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is not null && Object is Vector3Int Vector && this == Vector;
        }
        public override readonly int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + x;
                hash = hash * 23 + y;
                hash = hash * 23 + z;
                return hash;
            }
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(x);
            Writer.Write(y);
            Writer.Write(z);
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
            return new Vector3Int(Random.Next(Min.x, Max.x), Random.Next(Min.y, Max.y), Random.Next(Min.z, Max.z));
        }

        public void MoveUp() => y++;
        public void MoveDown() => y--;
        public void MoveRight() => x++;
        public void MoveLeft() => x--;
        public void MoveForward() => z++;
        public void MoveBack() => z--;

        public void Normalize() => this = Normal;


        public static double Distance(Vector3Int A, Vector3Int B)
        {
            Vector3Int Difference = A - B;

            return Math.Sqrt
            (
                (Difference.x * Difference.x) +
                (Difference.y * Difference.y) +
                (Difference.z * Difference.z)
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


        public static Vector3Int Absolute(Vector3Int Vector)
        {
            return new Vector3Int(Math.Abs(Vector.x), Math.Abs(Vector.y), Math.Abs(Vector.z));
        }
        public static Vector3Int Clamp(Vector3Int Value, Vector3Int Min, Vector3Int Max)
        {
            return new Vector3Int
            (
                Math.Clamp(Value.x, Min.x, Max.x),
                Math.Clamp(Value.y, Min.y, Max.y),
                Math.Clamp(Value.z, Min.z, Max.z)
            );
        }
    }
}