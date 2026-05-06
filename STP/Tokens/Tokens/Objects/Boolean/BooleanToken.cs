namespace Zion.STP
{
    public readonly struct BooleanToken : IValueToken<bool>
    {
        public int Length { get; init; }
        public bool Value { get; init; }
    }

    public readonly struct BooleanTokenReader : ITokenReader
    {
        public string True { get; init; } = "true";
        public string False { get; init; } = "false";

        public BooleanTokenReader() { }

        public bool Read(ref ITextSource Source, out IToken Token)
        {
            if (Source.Begins(True, out Source))
            {
                Token = new BooleanToken()
                {
                    Value = true,
                    Length = True.Length
                };
                return true;
            }
            if (Source.Begins(False, out Source))
            {
                Token = new BooleanToken()
                {
                    Value = false,
                    Length = False.Length
                };
                return true;
            }

            Token = default!;
            return false;
        }
    }
}