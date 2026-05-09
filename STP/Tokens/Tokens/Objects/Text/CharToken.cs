using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Zion.STP
{
    public readonly struct CharToken : IValueToken<char>
    {
        public int Length { get; init; }
        public char Value { get; init; }
        public TokenStatus Status { get; init; }

        public override string ToString() => $"['{Value}']";
    }

    public readonly struct CharTokenReader : ITokenReader
    {
        private static readonly Dictionary<char, char> EscapeChars = new()
        {
            { '\\', '\\' },

            { '"', '\"' },
            { '0', '\0' },
            { 'a', '\a' },
            { 'b', '\b' },
            { 'f', '\f' },
            { 't', '\t' },
            { 'v', '\v' },
            { 'r', '\r' },
            { 'n', '\n' }
        };

        public bool Read(ref TextSource Source, out IToken Token)
        {
            Token = default!;

            if (Source.IsEnd || Source.Current != '\'')
            {
                return false;
            }

            Source.MoveNext();

            if (Source.IsEnd || Source.Current == '\'')
            {
                Source.MoveNext();
                Token = new CharToken() { Length = 2, Status = TokenStatus.HasErrors };
                return true;
            }

            if (Source.Current == '\\')
            {
                return !Source.IsEnd && ReadEscapeChar(ref Source, out Token);
            }
            else
            {
                char Value = Source.Current;

                Source.MoveNext();

                if (Source.CurrentIs('\''))
                {
                    Source.MoveNext();
                    Token = new CharToken() { Length = 3, Value = Value };
                    return true;
                }
            }

            return false;
        }

        private bool ReadEscapeChar(ref TextSource Source, out IToken Token)
        {
            Source.MoveNext();

            if (Source.IsEnd)
            {
                Token = default!;
                return false;
            }

            switch (Source.Current)
            {
                case 'u':
                    return ReadHexFormat(ref Source, out Token, 4);

                case 'U':
                    return ReadHexFormat(ref Source, out Token, 8);

                case 'x':
                    return ReadHexFormat(ref Source, out Token);

                default:
                    return ReadUnicodeChar(ref Source, out Token);
            }
        }

        private static bool ReadHexFormat(ref TextSource Source, out IToken Token, int HexCodeLength)
        {
            Source.MoveNext();

            int HexCode = 0;

            if (Source.TryRead(HexCodeLength, out string HexCodeString, out Source)
                && Source.CurrentIs('\''))
            {
                Source.MoveNext();

                Token = TryParseHexCode(HexCodeString, out HexCode)
                    ? new CharToken() { Length = HexCodeLength + 4, Value = (char)HexCode }
                    : new CharToken() { Length = HexCodeLength + 4, Status = TokenStatus.HasErrors };
                return true;
            }

            Token = default!;
            return false;
        }

        private static bool ReadHexFormat(ref TextSource Source, out IToken Token)
        {
            Token = default!;

            Source.MoveNext();

            int HexCode = 0;
            int Readed = 0;

            while (!Source.IsEnd && Readed <= 4)
            {
                if (Source.Current == '\'')
                {
                    Token = new CharToken() { Length = 4 + Readed, Value = (char)HexCode };
                    return true;
                }

                if (Source.Current.IsHexadecimalDigit(out int Digit))
                {
                    Readed++;
                    HexCode <<= 4;
                    HexCode |= Digit;
                }
                else
                {
                    return false;
                }

                Source.MoveNext();
            }

            return false;
        }

        private static bool ReadUnicodeChar(ref TextSource Source, out IToken Token)
        {
            char Char = Source.Current;

            if (Char == '\'')
            {
                Source.MoveNext();

                if (Source.CurrentIs('\''))
                {
                    Source.MoveNext();
                    Token = new CharToken() { Length = 3, Value = '\'' };
                }
                else
                {
                    Token = new CharToken() { Length = 3, Value = '\\' };
                }

                return true;
            }

            Source.MoveNext();
            if (!Source.CurrentIs('\''))
            {
                Token = default!;
                return false;
            }

            Token = EscapeChars.TryGetValue(Char, out char UnicodeChar)
                ? new CharToken() { Length = 4, Value = UnicodeChar }
                : new CharToken() { Length = 4, Status = TokenStatus.HasErrors };
            return true;
        }

        private static bool TryParseHexCode(IEnumerable<char> Chars, out int HexCode)
        {
            HexCode = 0;

            foreach (char Char in Chars)
            {
                if (Char.IsHexadecimalDigit(out int Digit))
                {
                    HexCode <<= 4;
                    HexCode |= Digit;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}