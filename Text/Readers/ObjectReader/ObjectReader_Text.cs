using System.Text;

namespace Zion
{
    public sealed partial class ObjectReader //_Text
    {
        private static readonly Dictionary<char, char> EscapeChars = new()
        {
            { '\'', '\'' },
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

        public bool TryReadChar(out char Value)
        {
            int Index = this.Index;

            if (Index < Text.Length && Text[Index++] == '\''
                && TryReadChar(ref Index, out Value, false)
                && Index < Text.Length && Text[Index++] == '\'')
            {
                this.Index = Index;
                return true;
            }
            Value = default;
            return false;
        }

        public bool TryReadString(out string Value)
        {
            Value = null!;

            int Index = this.Index;

            StringBuilder Builder = new StringBuilder();

            if (Index >= Text.Length || Text[Index++] != '"') { return false; }

            while (Index < Text.Length)
            {
                if (Text[Index] == '"')
                {
                    Value = Builder.ToString();
                    this.Index = Index + 1;
                    return true;
                }

                if (TryReadChar(ref Index, out char Char, true))
                {
                    Builder.Append(Char);
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public char ReadChar()
        {
            return Unsafe<char>(TryReadChar);
        }

        public string ReadString()
        {
            return Unsafe<string>(TryReadString);
        }


        private bool TryReadChar(ref int Index, out char Char, bool IsString)
        {
            Char = default;

            if (Index >= Text.Length) { return false; }

            char Current = Text[Index];

            if (Current == '\\')
            {
                Index++;

                if (Index >= Text.Length) { return false; }

                Current = Text[Index];

                if (IsString && Current == '\'')
                {
                    Char = '\'';
                    Index++;
                    return true;
                }
                if (!IsString && Current == '\"')
                {
                    Char = '\"';
                    Index++;
                    return true;
                }


                switch (Current)
                {
                    case 'u':
                        if (TryReadHexCode(++Index, 4, out int uHexCode))
                        {
                            Char = (char)uHexCode;
                            Index += 4;
                            return true;
                        }
                        return false;

                    case 'U':
                        if (TryReadHexCode(++Index, 8, out int UHexCode))
                        {
                            Char = (char)UHexCode;
                            Index += 8;
                            return true;
                        }
                        return false;

                    case 'x':

                        Index++;

                        int StartIndex = Index;
                        int xHexCode = 0;
                        int DigitCount = 0;

                        while (Index < Text.Length && DigitCount < 4)
                        {
                            if (IsHexadecimal(Text[Index], out int Digit))
                            {
                                xHexCode <<= 4;
                                xHexCode |= Digit;

                                DigitCount++;
                                Index++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (DigitCount == 0)
                        {
                            Index = StartIndex;
                            return false;
                        }

                        Char = (char)xHexCode;
                        return true;

                    default:
                        if (EscapeChars.TryGetValue(Current, out char UnicodeChar))
                        {
                            Char = UnicodeChar;
                            Index++;
                            return true;
                        }

                        return false;
                }
            }
            else
            {
                Char = Current;
                Index++;
                return true;
            }
        }

        private bool TryReadHexCode(int Start, int CharCount, out int HexCode)
        {
            HexCode = 0;
            
            if (Start + CharCount > Text.Length) { return false; }

            for (int i = 0; i < CharCount; i++)
            {
                if (IsHexadecimal(Text[Start + i], out int Digit))
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