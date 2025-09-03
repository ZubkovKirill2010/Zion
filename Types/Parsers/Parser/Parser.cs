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
            return float.Parse(NormalizeToFloat(String));
        }
        public static bool ToFloat(this string String, out float Value)
        {
            if (String == "_")
            {
                Value = 0;
                return true;
            }
            return float.TryParse(NormalizeToFloat(String), out Value);
        }

        public static double ToDouble(this string String)
        {
            if (String == "_")
            {
                return 0;
            }
            return double.Parse(NormalizeToFloat(String));
        }
        public static bool ToDouble(this string String, out double Value)
        {
            if (String == "_")
            {
                Value = 0d;
                return true;
            }
            return double.TryParse(NormalizeToFloat(String), out Value);
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


        private static string Normalize(string String)
        {
            return String.RemoveChars
            (
                Char => char.IsWhiteSpace(Char) || Char == '_'
            );
        }
        private static string NormalizeToFloat(string String)
        {
            return Normalize(String).Replace('.', ',');
        }
    }
}