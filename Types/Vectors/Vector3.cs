using System.Diagnostics.CodeAnalysis;

namespace Zion.Vectors
{
    [Serializable]
    public struct Vector3 : IBinaryObject<Vector3>, IRandomizable<Vector3>
    {
        public static readonly Vector3 Zero = new Vector3(0);
        public static readonly Vector3 Up = new Vector3(0, 1, 0);
        public static readonly Vector3 Right = new Vector3(1, 0, 0);
        public static readonly Vector3 Down = new Vector3(0, -1, 0);
        public static readonly Vector3 Left = new Vector3(-1, 0, 0);
        public static readonly Vector3 Forward = new Vector3(0, 0, 1);
        public static readonly Vector3 Back = new Vector3(0, 0, -1);

        public float x, y, z;

        public Vector3 Normal
        {
            get
            {
                float Magnitude = MathF.Sqrt(x * x + y * y + z * z);
                return new Vector3(x / Magnitude, y / Magnitude, z / Magnitude);
            }
        }


        public Vector3(float Axis)
        {
            x = Axis;
            y = Axis;
            z = Axis;
        }
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3 operator --(Vector3 Vector)
        {
            Vector.x--;
            Vector.y--;
            Vector.z--;
            return Vector;
        }
        public static Vector3 operator ++(Vector3 Vector)
        {
            Vector.x++;
            Vector.y++;
            Vector.z++;
            return Vector;
        }

        public static Vector3 operator +(Vector3 A, Vector3 B)
        {
            return new Vector3(A.x + B.x, A.y + B.y, A.z + B.z);
        }
        public static Vector3 operator -(Vector3 A, Vector3 B)
        {
            return new Vector3(A.x - B.x, A.y - B.y, A.z - B.z);
        }
        public static Vector3 operator *(Vector3 A, float B)
        {
            return new Vector3(A.x * B, A.y * B, A.z * B);
        }
        public static Vector3 operator /(Vector3 A, float B)
        {
            return new Vector3(A.x / B, A.y / B, A.z / B);
        }

        public static bool operator ==(Vector3 A, Vector3 B)
        {
            return A.x == B.x && A.y == B.y && A.z == B.z;
        }
        public static bool operator !=(Vector3 A, Vector3 B)
        {
            return A.x != B.x || A.y != B.y || A.z != B.z;
        }

        public static bool operator ==(Vector3 A, Vector3Int B)
        {
            return A.x == B.x && A.y == B.y && A.z == B.z;
        }
        public static bool operator !=(Vector3 A, Vector3Int B)
        {
            return A.x != B.x || A.y != B.y || A.z != B.z;
        }

        public static bool operator ==(Vector3Int A, Vector3 B)
        {
            return A.x == B.x && A.y == B.y && A.z == B.z;
        }
        public static bool operator !=(Vector3Int A, Vector3 B)
        {
            return A.x != B.x || A.y != B.y || A.z != B.z;
        }

        public static bool operator <=(Vector3 A, Vector3 B)
        {
            return A.x <= B.x && A.y <= B.y && A.z <= B.z;
        }
        public static bool operator >=(Vector3 A, Vector3 B)
        {
            return A.x >= B.x && A.y >= B.y && A.y <= B.y;
        }


        public override readonly string ToString() => $"[{x}, {y}, {z}]";
        public override readonly bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is not null && Object is Vector3 Vector && this == Vector;
        }
        public override readonly int GetHashCode()
        {
            unchecked
            {
                int Hash = 17;
                Hash = Hash * 23 + x.GetHashCode();
                Hash = Hash * 23 + y.GetHashCode();
                Hash = Hash * 23 + z.GetHashCode();
                return Hash;
            }
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(x);
            Writer.Write(y);
            Writer.Write(z);
        }
        public static Vector3 Read(BinaryReader Reader)
        {
            return new Vector3
            (
                Reader.ReadSingle(),
                Reader.ReadSingle(),
                Reader.ReadSingle()
            );
        }

        public static Vector3 GetRandom(Random Random, Vector3 Min, Vector3 Max)
        {
            return new Vector3(Random.NextFloat(Min.x, Max.x), Random.NextFloat(Min.y, Max.y), Random.NextFloat(Min.z, Max.z));
        }


        public void MoveUp() => y++;
        public void MoveDown() => y--;
        public void MoveRight() => x++;
        public void MoveLeft() => x--;
        public void MoveForward() => z++;
        public void MoveBack() => z--;

        public void Normalize() => this = Normal;


        public static double Distance(Vector3 A, Vector3 B)
        {
            Vector3 Difference = A - B;

            return Math.Sqrt
            (
                (Difference.x * Difference.x) +
                (Difference.y * Difference.y) +
                (Difference.z * Difference.z)
            );
        }

        public static bool CompareDistance(Vector3 A, Vector3 B, float MaxDistance)
        {
            float DiferenceX = A.x - B.x;
            float DiferenceY = A.y - B.y;
            float DiferenceZ = A.z - B.z;
            return DiferenceX * DiferenceX + DiferenceY * DiferenceY + DiferenceZ * DiferenceZ <= MaxDistance * MaxDistance;
        }

        public static Vector3 Lerp(Vector3 A, Vector3 B, float Alpha)
        {
            return A + (B - A) * Alpha;
        }

        public static Vector3 Sum(params IEnumerable<Vector3> Vectors)
        {
            ArgumentNullException.ThrowIfNull(Vectors);

            Vector3 Result = new Vector3();

            foreach (Vector3 Vector in Vectors)
            {
                Result += Vector;
            }

            return Result;
        }


        public static Vector3 Abs(Vector3 Vector)
        {
            return new Vector3(Math.Abs(Vector.x), Math.Abs(Vector.y), Math.Abs(Vector.z));
        }
        public static Vector3 Clamp(Vector3 Value, Vector3 Min, Vector3 Max)
        {
            return new Vector3
            (
                Math.Clamp(Value.x, Min.x, Max.x),
                Math.Clamp(Value.y, Min.y, Max.y),
                Math.Clamp(Value.z, Min.z, Max.z)
            );
        }
    }
}