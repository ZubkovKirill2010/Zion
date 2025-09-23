using System.Text;

namespace Zion
{
    [Serializable]
    public struct ColorText : IColorText
    {
        public static readonly ColorText Empty = new ColorText(string.Empty, new Color(0));

        public string Text;
        public Color Color;

        public int Length => Text.Length;

        public ColorText(object? Text, Color Color)
        {
            this.Text = (Text ?? string.Empty).ToString() ?? string.Empty;
            this.Color = Color;
        }

        public ColorChar this[int Index] => new ColorChar(Text[Index], Color);

        public override string ToString()
        {
            return $"\u001b[38;2;{Color.R};{Color.G};{Color.B}m{Text}\u001b[0m";
        }
        public string ToString(out int VisualLength)
        {
            VisualLength = Length;
            return $"\u001b[38;2;{Color.R};{Color.G};{Color.B}m{Text}\u001b[0m";
        }

        public string ToUnicode()
        {
            return $"\\u001b[38;2;{Color.R};{Color.G};{Color.B}m{Text}\\u001b[0m";
        }
        public static string ToUnicode(string Text, Color Color)
        {
            return $"\\u001b[38;2;{Color.R};{Color.G};{Color.B}m{Text}\\u001b[0m";
        }

        public string ToColorString() => ToString();

        public static string GetText(string Text, Color Color)
        {
            return $"\u001b[38;2;{Color.R};{Color.G};{Color.B}m{Text}\u001b[0m";
        }
        public static string GetText(string Text, byte R, byte G, byte B)
        {
            return $"\u001b[38;2;{R};{G};{B}m{Text}\u001b[0m";
        }

        public static string Clear(string String)
        {
            int Start = 0;
            int Index = 0;
            StringBuilder Builder = new StringBuilder(String.Length);

            while (Index < String.Length)
            {
                if (String.Begins(Index, "\u001b[38;2;"))
                {
                    Index += 7;
                    Index = String.Skip(Index, Char => Char != ';') + 1;
                    Index = String.Skip(Index, Char => Char != ';') + 1;
                    Index = String.Skip(Index, Char => Char != 'm') + 1;

                    Start = Index;

                    Index = String.Skip(Index, Char => Char != '\u001b');

                    Builder.Append(String[Start..Index]);

                    if (Index < String.Length && String[Index] == '\u001b')
                    {
                        Index = String.Skip(Index, Char => Char != 'm') + 1;
                    }
                }
                else if (String[Index] != '\u001b')
                {
                    Builder.Append(String[Index]);
                    Index++;
                }
                else
                {
                    Index++;
                }
            }
            return Builder.ToString();
        }

        public static bool IsColored(string String)
        {
            return String.Contains("\u001b[38;2;");
        }

        public static string ConcatToString(params ColorText[] Array)
        {
            return string.Concat(Array);
        }
        public static MassColorText Concat(out int VisualLength, params ColorText[] Array)
        {
            MassColorText Result = new MassColorText(Array);
            VisualLength = Result.Length;
            return Result;
        }

        public static string operator +(ColorText A, ColorText B)
        {
            return A.ToString() + B.ToString();
        }
        public static string operator +(ColorText A, ColorChar B)
        {
            return A.ToString() + B.ToString();
        }
        public static string operator +(ColorChar A, ColorText B)
        {
            return A.ToString() + B.ToString();
        }
    }
}