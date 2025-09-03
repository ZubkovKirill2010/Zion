using System.Diagnostics.CodeAnalysis;

namespace Zion.Vectors
{
    public struct Vector2Byte
    {
        public static readonly Vector2Byte Zero = new Vector2Byte(0, 0);
        public static readonly Vector2Byte Up = new Vector2Byte(0, 1);
        public static readonly Vector2Byte Right = new Vector2Byte(1, 0);
        public static readonly Vector2Byte Down = new Vector2Byte(0, -1);
        public static readonly Vector2Byte Left = new Vector2Byte(-1, 0);
        public static readonly Vector2Byte OneOne = new Vector2Byte(1);

        public const int MinValue = -3;
        public const int MaxValue = 4;

        private byte Value;

        public int x
        {
            get => ConvertBits(Value & 0b0000_1111);
            set
            {
                if (IsInside(value))
                {
                    Value = (byte)((Value & 0b1111_0000) | ConvertToBits(value));
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"new x(={value}) out of Range -3 - 4");
                }
            }
        }
        public int y
        {
            get => ConvertBits(Value >> 4);
            set
            {
                if (IsInside(value))
                {
                    Value = (byte)((Value & 0b0000_1111) | (ConvertToBits(value) << 4));
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"new y(={value}) out of Range -3 - 4");
                }
            }
        }


        public Vector2Byte(int Axis)
        {
            if (IsInside(Axis))
            {
                byte Bits = ConvertToBits(Axis);
                Value = (byte)(Bits | (Bits << 4));
            }
            else
            {
                throw new ArgumentOutOfRangeException($"new y(={Axis}) out of Range -3 - 4");
            }
        }
        public Vector2Byte(int x, int y)
        {
            if (!IsInside(x)) { throw new ArgumentOutOfRangeException(nameof(x)); };
            if (!IsInside(y)) { throw new ArgumentOutOfRangeException(nameof(y)); };

            Value = (byte)((ConvertToBits(y) << 4) | ConvertToBits(x));
        }


        public static bool operator ==(Vector2Byte A, Vector2Byte B) => A.Value == B.Value;
        public static bool operator !=(Vector2Byte A, Vector2Byte B) => A.Value != B.Value;

        public static bool operator ==(Vector2Byte A, Vector2Int B) => A.x == B.x && A.y == B.y;
        public static bool operator !=(Vector2Byte A, Vector2Int B) => A.x != B.x || A.y != B.y;

        public static bool operator ==(Vector2Int A, Vector2Byte B) => A.x == B.x && A.y == B.y;
        public static bool operator !=(Vector2Int A, Vector2Byte B) => A.x != B.x || A.y != B.y;

        public static bool operator ==(Vector2Byte A, Vector2 B) => A.x == B.x && A.y == B.y;
        public static bool operator !=(Vector2Byte A, Vector2 B) => A.x != B.x || A.y != B.y;

        public static bool operator ==(Vector2 A, Vector2Byte B) => A.x == B.x && A.y == B.y;
        public static bool operator !=(Vector2 A, Vector2Byte B) => A.x != B.x || A.y != B.y;

        public static bool operator <(Vector2Byte A, Vector2Byte B) => A.x < B.x && A.y < B.y;
        public static bool operator >(Vector2Byte A, Vector2Byte B) => A.x > B.x && A.y > B.y;
        public static bool operator <=(Vector2Byte A, Vector2Byte B) => A.x <= B.x && A.y <= B.y;
        public static bool operator >=(Vector2Byte A, Vector2Byte B) => A.x >= B.x && A.y >= B.y;

        public static explicit operator Vector2Int(Vector2Byte Vector) => new Vector2Int(Vector.x, Vector.y);


        public override string ToString() => $"[{x}, {y}]";
        public override int GetHashCode() => Value;
        public override bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is not null && Object is Vector2Byte Vector2Byte ?
                this == Vector2Byte : false;
        }


        public static bool IsInside(int Value)
        {
            return Value >= MinValue && Value <= MaxValue;
        }


        private int ConvertBits(int Value)
        {
            int Magnitude = Value & 0b0111;
            return (Value & 0b1000) != 0 ? -Magnitude : Magnitude;
        }

        private byte ConvertToBits(int value)
        {
            return value < 0 ?
                (byte)(0b1000 | (Math.Abs(value) & 0b0111)) :
                (byte)(value & 0b0111);
        }
    }
}