namespace Zion.STP
{
    public interface IDigitParser
    {
        public char BinaryPrefix { get; }

        public char OctalPrefix { get; }

        public char HexadecimalPrefix { get; }


        public bool IsBinary(char Char, out int Digit);

        public bool IsOctal(char Char, out int Digit);

        public bool IsDecimal(char Char, out int Digit);

        public bool IsHexadecimal(char Char, out int Digit);
    }
}