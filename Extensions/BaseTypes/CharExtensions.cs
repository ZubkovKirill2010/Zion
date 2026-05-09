namespace Zion
{
    public static class CharExtensions
    {
        extension(char Char)
        {
            public bool IsBinaryDigit()
            {
                return Char == '0' || Char == '1';
            }

            public bool IsOctalDigit()
            {
                return Char >= '0' && Char <= '7';
            }

            public bool IsDigit()
            {
                return Char >= '0' && Char <= '9';
            }

            public bool IsHexadecimalDigit()
            {
                return Char >= '0' && Char <= '9'
                    || Char >= 'a' && Char <= 'f'
                    || Char >= 'A' && Char <= 'F';
            }


            public bool IsBinaryDigit(out int Digit)
            {
                if (Char == '0')
                {
                    Digit = 0;
                    return true;
                }
                if (Char == '1')
                {
                    Digit = 1;
                    return true;
                }
                Digit = default;
                return false;
            }

            public bool IsOctalDigit(out int Digit)
            {
                if (Char >= '0' && Char <= '7')
                {
                    Digit = Char - '0';
                    return true;
                }
                Digit = default;
                return false;
            }

            public bool IsDigit(out int Digit)
            {
                if (Char >= '0' && Char <= '9')
                {
                    Digit = Char - '0';
                    return true;
                }
                Digit = default;
                return false;
            }

            public bool IsHexadecimalDigit(out int Digit)
            {
                if (Char >= '0' && Char <= '9')
                {
                    Digit = Char - '0';
                    return true;
                }
                if (Char >= 'a' && Char <= 'f')
                {
                    Digit = Char - 'a' + 10;
                    return true;
                }
                if (Char >= 'A' && Char <= 'F')
                {
                    Digit = Char - 'A' + 10;
                    return true;
                }
                Digit = default;
                return false;
            }

            /// <summary>
            /// Checks if the character matches any of the specified characters.
            /// </summary>
            /// <param name="Char">The character to check.</param>
            /// <param name="Chars">The characters to compare against.</param>
            /// <returns>True if the character matches any of the provided characters, otherwise false.</returns>
            public bool Is(params char[] Chars)
            {
                return Chars.Contains(Char);
            }
        }        
    }
}