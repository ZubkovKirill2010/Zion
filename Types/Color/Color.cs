using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Zion.Vectors;

namespace Zion
{
    [Serializable]
    public struct Color : IBinaryObject<Color>, IEnumerable<byte>
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
        public static readonly Color Violet = new Color(144, 40, 241);


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
            return $"({R}, {G}, {B})";
        }
        public override bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is Color Color && this == Color;
        }


        public static Color Parse(string String)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(String);

            if (String.StartsWith('#') && String.Length == 7)
            {
                if (String.Length == 3)
                {
                    return new Color
                    (
                        HexToByte(String[1..3], "RGB")
                    );
                }

                if (String.Length == 7)
                {
                    byte R = HexToByte(String[1..3], 'R');
                    byte G = HexToByte(String[3..5], 'G');
                    byte B = HexToByte(String[5..7], 'B');

                    return new Color(R, G, B);
                }
            }

            byte[] Result = new byte[3];

            int Start = 0;
            int Index = 0;

            try
            {
                for (int i = 0; i < 3; i++)
                {
                    Index = String.Skip(Index, Char => !char.IsDigit(Char));
                    Start = Index;
                    Index = String.Skip(Index, char.IsDigit);

                    Result[i] = byte.Parse(String[Start..Index]);
                }
            }
            catch
            {
                throw new FormatException($"Couldn't convert String(=\"{String}\") to Color");
            }
            return new Color(Result[0], Result[1], Result[2]);
        }
        public static bool TryParse(string String, out Color Color)
        {
            try
            {
                Color = Parse(String);
                return true;
            }
            catch
            {
                Color = default;
                return false;
            }
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


        public string ToHexString()
        {
            string Format = "X2";
            return $"#{R.ToString(Format)}{G.ToString(Format)}{B.ToString(Format)}";
        }

        public readonly float GetBrightness()//Range 0 - 255
        {
            return R * 0.2126f + G * 0.7152f + B * 0.0722f;
        }
        public readonly Color GetOpposite()
        {
            return new Color((byte)(byte.MaxValue - R), (byte)(byte.MaxValue - G), (byte)(byte.MaxValue - B));
        }


        private static byte HexToByte(string String, object Identifier)
        {
            try
            {
                return Convert.ToByte(String, 16);
            }
            catch
            {
                throw new FormatException($"Incorrect hex format(={String}) of ({Identifier})");
            }
        }

        public IEnumerator<byte> GetEnumerator()
        {
            yield return R;
            yield return G;
            yield return B;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}