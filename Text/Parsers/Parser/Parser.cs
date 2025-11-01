using System.Numerics;

namespace Zion
{
    public static partial class Parser
    {
        public static int ToInt32(this string String)
        {
            if (String == "_")
            {
                return 0;
            }
            return int.Parse(Normalize(String));
        }
        public static bool ToInt32(this string String, out int Value)
        {
            if (String == "_")
            {
                Value = 0;
                return true;
            }
            return int.TryParse(Normalize(String), out Value);
        }

        public static float ToFloat(this string String)
        {
            if (String == "_")
            {
                return 0;
            }
            return float.Parse(NormalizeFloated(String));
        }
        public static bool ToFloat(this string String, out float Value)
        {
            if (String == "_")
            {
                Value = 0;
                return true;
            }
            return float.TryParse(NormalizeFloated(String), out Value);
        }

        public static double ToDouble(this string String)
        {
            if (String == "_")
            {
                return 0;
            }
            return double.Parse(NormalizeFloated(String));
        }
        public static bool ToDouble(this string String, out double Value)
        {
            if (String == "_")
            {
                Value = 0d;
                return true;
            }
            return double.TryParse(NormalizeFloated(String), out Value);
        }

        public static BigInteger ToBigInteger(this string String)
        {
            String = Normalize(String);

            bool IsNegative = String.StartsWith('-');
            int Start = 0;

            if (IsNegative || String.StartsWith('+'))
            {
                Start = 1;
            }

            BigInteger GetResult(BigInteger Value)
            {
                return IsNegative ? -Value : Value;
            }

            if (String.Begins(Start, "0b", true))
            {
                if (String.Length == Start + 2)
                {
                    return BigInteger.Zero;
                }

                BigInteger Result = 0;

                for (int i = Start + 2; i < String.Length; i++)
                {
                    Result <<= 1;
                    char Current = String[i];

                    if (Current == '1')
                    {
                        Result |= 1;
                    }
                    else if (Current != '0')
                    {
                        throw new FormatException($"Couldn't convert \"{String}\" to BigInteger");
                    }
                }
                return GetResult(Result);
            }

            if (String.Begins(Start, "0x", true))
            {
                if (String.Length == Start + 2)
                {
                    return BigInteger.Zero;
                }

                BigInteger Result = 0;

                for (int i = Start + 2; i < String.Length; i++)
                {
                    Result <<= 4;
                    Result |= ToHex(String[i]);
                }
                return GetResult(Result);
            }

            return BigInteger.Parse(String);
        }
        public static bool ToBigInteger(this string String, out BigInteger Value)
        {
            try
            {
                Value = ToBigInteger(String);
                return true;
            }
            catch
            {
                Value = default;
                return false;
            }
        }


        public static bool ToEnum<T>(this string String, out T? EnumValue) where T : Enum
        {
            foreach (object? Value in Enum.GetValues(typeof(T)))
            {
                string EnumString = Value.ToString();

                if (string.Compare(String, EnumString, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    EnumValue = (T)Value;
                    return true;
                }
            }

            EnumValue = default;
            return false;
        }


        public static int ToHex(char Char)
        {
            if (char.IsDigit(Char)) { return Char - '0'; }
            if (Char >= 'a' && Char <= 'f') { return Char - 'a' + 10; }
            if (Char >= 'A' && Char <= 'F') { return Char - 'A' + 10; }
            throw new FormatException($"Couldn't convert '{Char}' to hex");
        }


        internal static string Normalize(string String)
        {
            return String.RemoveChars
            (
                Char => char.IsWhiteSpace(Char) || Char == '_'
            );
        }
        internal static string NormalizeFloated(string String)
        {
            return Normalize(String).Replace('.', ',');
        }
    }
}