using System.Diagnostics.CodeAnalysis;
using Zion.Vectors;

namespace Zion
{
    [Serializable]
    public struct Color : IBinaryObject<Color>
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public static readonly Color Black = new Color(0);
        public static readonly Color DarkGray = new Color(128);
        public static readonly Color Gray = new Color(182);
        public static readonly Color White = new Color(255);

        public static readonly Color DarkBlue = new Color(0, 0, 255);
        public static readonly Color DarkGreen = new Color(85, 177, 85);
        public static readonly Color DarkCyan = new Color(0, 128, 128);
        public static readonly Color DarkRed = new Color(128, 0, 0);
        public static readonly Color DarkMagenta = new Color(128, 0, 128);
        public static readonly Color DarkYellow = new Color(128, 128, 0);

        public static readonly Color Blue = new Color(41, 161, 221);
        public static readonly Color Green = new Color(0, 255, 0);
        public static readonly Color Cyan = new Color(0, 255, 255);
        public static readonly Color Red = new Color(255, 0, 0);
        public static readonly Color Magenta = new Color(255, 0, 255);
        public static readonly Color Yellow = new Color(255, 255, 0);
        public static readonly Color Orange = new Color(247, 102, 3);


        public Color(byte Value)
        {
            R = Value;
            G = Value;
            B = Value;
        }
        public Color(byte R, byte G, byte B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }
        public Color(Vector3Int Color)
        {
            R = (byte)(Math.Abs(Color.x) % 256);
            G = (byte)(Math.Abs(Color.y) % 256);
            B = (byte)(Math.Abs(Color.z) % 256);
        }
        public Color(System.Drawing.Color Color)
        {
            R = Color.R;
            G = Color.G;
            B = Color.B;
        }

        public static bool operator ==(Color A, Color B)
        {
            return A.R == B.R && A.G == B.G && A.B == B.B;
        }
        public static bool operator !=(Color A, Color B)
        {
            return A.R != B.R || A.G != B.G || A.B != B.B;
        }
        public static Color operator *(Color A, float B)
        {
            return new Color((byte)(A.R * B), (byte)(A.G * B), (byte)(A.B * B));
        }

        public override readonly string ToString()
        {
            return $"({R}; {G}; {B})";
        }
        public override bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is Color Color && this == Color;
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(R);
            Writer.Write(G);
            Writer.Write(B);
        }
        public static Color Read(BinaryReader Reader)
        {
            return new Color
            (
                Reader.ReadByte(),
                Reader.ReadByte(),
                Reader.ReadByte()
            );
        }


        public string ToString16()
        {
            string Format = "X2";
            return $"#{R.ToString(Format)}{G.ToString(Format)}{B.ToString(Format)}";
        }

        public readonly float GetBrightness()
        {
            return R * 0.2126f + G * 0.7152f + B * 0.0722f;
        }
        public readonly Color GetOpposite()
        {
            return new Color((byte)(byte.MaxValue - R), (byte)(byte.MaxValue - G), (byte)(byte.MaxValue - B));
        }
    }
}