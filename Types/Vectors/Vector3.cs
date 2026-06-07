using System.Diagnostics.CodeAnalysis;

namespace Zion.Vectors
{
    [Serializable]
    public struct Vector3 : IBinarySerializable<Vector3>, IRandomizable<Vector3>
    {
        public static readonly Vector3 Zero = new Vector3(0);
        public static readonly Vector3 Up = new Vector3(0, 1, 0);
        public static readonly Vector3 Right = new Vector3(1, 0, 0);
        public static readonly Vector3 Down = new Vector3(0, -1, 0);
        public static readonly Vector3 Left = new Vector3(-1, 0, 0);
        public static readonly Vector3 Forward = new Vector3(0, 0, 1);
        public static readonly Vector3 Back = new Vector3(0, 0, -1);

        public float X, Y, Z;

        public Vector3 Normal
        {
            get
            {
                float Magnitude = MathF.Sqrt((X * X) + (Y * Y) + (Z * Z));
                return new Vector3(X / Magnitude, Y / Magnitude, Z / Magnitude);
            }
        }


        public Vector3(float Axis)
        {
            X = Axis;
            Y = Axis;
            Z = Axis;
        }
        public Vector3(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public static Vector3 operator --(Vector3 Vector)
        {
            Vector.X--;
            Vector.Y--;
            Vector.Z--;
            return Vector;
        }
        public static Vector3 operator ++(Vector3 Vector)
        {
            Vector.X++;
            Vector.Y++;
            Vector.Z++;
            return Vector;
        }

        public static Vector3 operator +(Vector3 A, Vector3 B)
        {
            return new Vector3(A.X + B.X, A.Y + B.Y, A.Z + B.Z);
        }
        public static Vector3 operator -(Vector3 A, Vector3 B)
        {
            return new Vector3(A.X - B.X, A.Y - B.Y, A.Z - B.Z);
        }
        public static Vector3 operator *(Vector3 A, float B)
        {
            return new Vector3(A.X * B, A.Y * B, A.Z * B);
        }
        public static Vector3 operator /(Vector3 A, float B)
        {
            return new Vector3(A.X / B, A.Y / B, A.Z / B);
        }

        public static bool operator ==(Vector3 A, Vector3 B)
        {
            return A.X == B.X && A.Y == B.Y && A.Z == B.Z;
        }
        public static bool operator !=(Vector3 A, Vector3 B)
        {
            return A.X != B.X || A.Y != B.Y || A.Z != B.Z;
        }

        public static bool operator ==(Vector3 A, Vector3Int B)
        {
            return A.X == B.X && A.Y == B.Y && A.Z == B.Z;
        }
        public static bool operator !=(Vector3 A, Vector3Int B)
        {
            return A.X != B.X || A.Y != B.Y || A.Z != B.Z;
        }

        public static bool operator ==(Vector3Int A, Vector3 B)
        {
            return A.X == B.X && A.Y == B.Y && A.Z == B.Z;
        }
        public static bool operator !=(Vector3Int A, Vector3 B)
        {
            return A.X != B.X || A.Y != B.Y || A.Z != B.Z;
        }

        public static bool operator <=(Vector3 A, Vector3 B)
        {
            return A.X <= B.X && A.Y <= B.Y && A.Z <= B.Z;
        }
        public static bool operator >=(Vector3 A, Vector3 B)
        {
            return A.X >= B.X && A.Y >= B.Y && A.Y <= B.Y;
        }


        public override readonly string ToString() => $"[{X}; {Y}; {Z}]";
        public override readonly bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is not null && Object is Vector3 Vector && this == Vector;
        }
        public override readonly int GetHashCode()
        {
            unchecked
            {
                int Hash = 17;
                Hash = (Hash * 23) + X.GetHashCode();
                Hash = (Hash * 23) + Y.GetHashCode();
                Hash = (Hash * 23) + Z.GetHashCode();
                return Hash;
            }
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(X);
            Writer.Write(Y);
            Writer.Write(Z);
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
            return new Vector3(Random.NextFloat(Min.X, Max.X), Random.NextFloat(Min.Y, Max.Y), Random.NextFloat(Min.Z, Max.Z));
        }


        public void MoveUp() => Y++;
        public void MoveDown() => Y--;
        public void MoveRight() => X++;
        public void MoveLeft() => X--;
        public void MoveForward() => Z++;
        public void MoveBack() => Z--;

        public void Normalize() => this = Normal;


        public static double Distance(Vector3 A, Vector3 B)
        {
            Vector3 Difference = A - B;

            return Math.Sqrt
            (
                (Difference.X * Difference.X) +
                (Difference.Y * Difference.Y) +
                (Difference.Z * Difference.Z)
            );
        }

        public static bool CompareDistance(Vector3 A, Vector3 B, float MaxDistance)
        {
            float DiferenceX = A.X - B.X;
            float DiferenceY = A.Y - B.Y;
            float DiferenceZ = A.Z - B.Z;
            return (DiferenceX * DiferenceX) + (DiferenceY * DiferenceY) + (DiferenceZ * DiferenceZ) <= MaxDistance * MaxDistance;
        }

        public static Vector3 Lerp(Vector3 A, Vector3 B, float Alpha)
        {
            return A + ((B - A) * Alpha);
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
            return new Vector3(Math.Abs(Vector.X), Math.Abs(Vector.Y), Math.Abs(Vector.Z));
        }
        public static Vector3 Clamp(Vector3 Value, Vector3 Min, Vector3 Max)
        {
            return new Vector3
            (
                Math.Clamp(Value.X, Min.X, Max.X),
                Math.Clamp(Value.Y, Min.Y, Max.Y),
                Math.Clamp(Value.Z, Min.Z, Max.Z)
            );
        }
    }
}