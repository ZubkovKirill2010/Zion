namespace Zion
{
    public static class CharExtensions
    {
        public static bool Is(this char Char, params char[] Chars)
        {
            return Chars.Contains(Char);
        }
    }
}