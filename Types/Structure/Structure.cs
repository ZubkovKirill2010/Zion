using Zion.Text;

namespace Zion
{
    public static class Structure
    {
        public static Structure<string> Parse(string Text, char OffsetChar = '\t')
        {
            return Parse(Text, Item => Item, OffsetChar);
        }
        public static Structure<string> Parse(string[] Lines, char OffsetChar = '\t')
        {
            return Parse(Lines, Item => Item, OffsetChar);
        }

        public static Structure<I> Parse<I>(string Text, Converter<string, I> Converter, char OffsetChar = '\t')
        {
            return Parse(Text.Split(Environment.NewLine), Converter, OffsetChar);
        }
        public static Structure<I> Parse<I>(string[] Lines, Converter<string, I> Converter, char OffsetChar = '\t')
        {
            if (Lines.Length <= 0)
            {
                return new Structure<I>(default);
            }

            int StartOffset = GetOffset(Lines[0], out _, OffsetChar);
            int Line = 0;

            return Parse(Lines, Converter, ref Line, StartOffset, StartOffset, OffsetChar);
        }


        private static Structure<I> Parse<I>(string[] Lines, Converter<string, I> Converter, ref int Line, int MinOffset, int TargetIndent, char OffsetChar)
        {
            int Offset = GetOffset(Lines[Line], out string Value, OffsetChar);

            Structure<I> Current = new Structure<I>(Converter(Value));
            Line++;

            while (Line < Lines.Length)
            {
                Offset = GetOffset(Lines[Line], out _, OffsetChar);

                if (Offset < MinOffset || Offset <= TargetIndent)
                {
                    break;
                }

                if (Offset >= TargetIndent)
                {
                    Current.Add(Parse(Lines, Converter, ref Line, MinOffset, Offset, OffsetChar));
                }
                else
                {
                    Line++;
                }
            }

            return Current;
        }

        private static int GetOffset(string Line, out string Value, char OffsetChar)
        {
            int Index = Line.Skip(0, Char => Char == OffsetChar);
            Value = Line[Index..];
            return Index;
        }


        public static Structure<string> UnParse(string Text)
        {
            return UnParse(Text, Item => Item);
        }
        public static Structure<string> UnParse(string[] Lines)
        {
            return UnParse(Lines, Item => Item);
        }

        public static Structure<I> UnParse<I>(string Text, Converter<string, I> Converter)
        {
            return UnParse(Text.Split(Environment.NewLine), Converter);
        }
        public static Structure<I> UnParse<I>(string[] Lines, Converter<string, I> Converter)
        {
            if (Lines.Length <= 0)
            {
                return new Structure<I>(default);
            }

            int StartOffset = GetLinesOffset(Lines[0], out _);
            int Line = 0;

            return UnParse(Lines, Converter, ref Line, StartOffset, StartOffset);
        }


        private static Structure<I> UnParse<I>(string[] Lines, Converter<string, I> Converter, ref int Line, int MinOffset, int TargetIndent)
        {
            int Offset = GetLinesOffset(Lines[Line], out string Value);

            Structure<I> Current = new Structure<I>(Converter(Value));
            Line++;

            while (Line < Lines.Length)
            {
                Offset = GetLinesOffset(Lines[Line], out _);

                if (Offset < MinOffset || Offset <= TargetIndent)
                {
                    break;
                }

                if (Offset >= TargetIndent)
                {
                    Current.Add(UnParse(Lines, Converter, ref Line, MinOffset, Offset));
                }
                else
                {
                    Line++;
                }
            }

            return Current;
        }

        private static int GetLinesOffset(string Line, out string Value)
        {
            int Offset = 0;

            while (Offset < Line.Length)
            {
                if
                (
                    Offset + 2 < Line.Length &&
                    (Line[Offset] == '├' || Line[Offset] == '└')
                    && Line[Offset + 1] == '─' &&
                    Line[Offset + 2] == '─'
                )
                {
                    Offset += 3;
                }
                else if (Line[Offset] == '│' || Line[Offset] == ' ')
                {
                    Offset++;
                }
                else
                {
                    break;
                }
            }

            Value = Line[Offset..];
            return Offset;
        }
    }
}