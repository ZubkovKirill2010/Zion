namespace Zion
{
    public static class CharExtensions
    {
        /// <summary>
        /// Checks if the character matches any of the specified characters.
        /// </summary>
        /// <param name="Char">The character to check.</param>
        /// <param name="Chars">The characters to compare against.</param>
        /// <returns>True if the character matches any of the provided characters, otherwise false.</returns>
        public static bool Is(this char Char, params char[] Chars)
        {
            return Chars.Contains(Char);
        }
    }
}