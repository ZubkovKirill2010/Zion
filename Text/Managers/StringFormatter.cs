namespace Zion
{
    public enum NumberFormat
    {
        Decimal, Hexadecimal, Binary
    }

    public static class StringFormatter
    {
        public static string ToString<T>(IEnumerable<T> Enumerable)
        {
            return $"[{string.Join(", ", Enumerable.Select(static Item => Item?.ToString() ?? "null"))}]";
        }

        public static string ToString(IEnumerable<string> Enumerable)
        {
            static string FormatString(string? String)
            {
                return String is null ? "null" : $"\"{String}\"";
            }

            return $"[{string.Join(", ", Enumerable.Select(FormatString))}]";
        }
    }
}