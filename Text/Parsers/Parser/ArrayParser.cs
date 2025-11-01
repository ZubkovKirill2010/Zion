using Zion.MathExpressions;

namespace Zion
{
    public static partial class Parser
    {
        private static T[] ToArray<T>(string String, Converter<string, string> NormalizeString, Converter<Fraction, T> ConvertValue, Converter<string, T> Parse)
        {
            if (string.IsNullOrWhiteSpace(String) || String.Length <= 2)
            {
                return Array.Empty<T>();
            }
            if (!String.StartsWith('[') || !String.EndsWith(']'))
            {
                throw new FormatException("Array must have the format \"[Value1, Value2, ..., ValueN]\"");
            }

            String = NormalizeString(String);

            List<T> Result = new List<T>(String.Length / 3);
            int Index = 1;
            int Start = 0;
            int End = String.Length - 1;

            while (Index < End)
            {
                char Char = String[Index];

                if (Char == '(')
                {
                    Start = Index + 1;
                    Index = StringParser.GetEndOfBracketsExpression(String, Index);

                    if (Index == -1)
                    {
                        throw new FormatException("No closing brackets");
                    }
                    if (Start + 1 == Index)
                    {
                        throw new FormatException("Couldn't convert \"()\" to Int32");
                    }

                    T Value = ConvertValue
                    (
                        new MathExpressionParser(String[Start..Index]).Parse().GetValue()
                    );

                    Result.Add(Value);

                    Index += 2;
                }
                else
                {
                    if (!char.IsDigit(Char) && Char != '-')
                    {
                        throw new FormatException($"Parsing error at position [{Result.Count}] {{ Char = '{Char}' }}");
                    }
                    Start = Index;
                    Index = String.Skip(Index + 1, char.IsDigit);

                    Result.Add(Parse(String[Start..Index]));

                    Index++;
                }
            }

            return Result.ToArray();
        }

        //public static int[] ToIntArray(this string String)
        //    => ToArray(String, Normalize, Fraction => Fraction.ToInt(), int.Parse);

        //public static float[] ToFloatArray(this string String)
        //    => ToArray(String, NormalizeFloated, Fraction => Fraction.ToFloat(), float.Parse);

        //public static double[] ToDoubleArray(this string String)
        //    => ToArray(String, NormalizeFloated, Fraction => Fraction.ToDouble(), double.Parse);

        //public static decimal[] ToDecimalArray(this string String)
        //    => ToArray(String, NormalizeFloated, Fraction => Fraction.ToDecimal(), decimal.Parse);


        public static string[] ToStringArray(this string String)
        {
            if (string.IsNullOrWhiteSpace(String) || String.Length <= 2)
            {
                return Array.Empty<string>();
            }
            if (!String.StartsWith('[') || !String.EndsWith(']'))
            {
                throw new FormatException("Array must have the format \"[Value1, Value2, ..., ValueN]\"");
            }

            List<string> Result = new List<string>();
            int Index = 1;
            int Start = 0;

            while (Index < String.Length)
            {
                Index = String.SkipSpaces(Index);

                if (String[Index] != '"')
                {
                    throw new FormatException($"The string should start with '\"' (Element index={Result.Count})");
                }

                Start = Index + 1;
                Index = StringParser.GetEndOfExpression(String, Index);

                Result.Add(StringParser.Parse(String[Start..Index]));

                Index = String.SkipSpaces(Index);

                if (Index >= String.Length || String[Index] == ']')
                {
                    break;
                }

                Index = String.Skip(',');
            }

            return Result.ToArray();
        }
    }
}