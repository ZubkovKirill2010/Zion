namespace Zion.STP
{
    public readonly struct EnumToken<T> : IValueToken<T> where T : Enum
    {
        public int Length { get; init; }
        public T   Value  { get; init; }
        public TokenStatus Status { get; init; }

        public override string ToString() => $"[{Value}]";
    }

    public readonly struct EnumTokenReader<T> : ITokenReader where T : struct, Enum
    {
        private readonly Dictionary<string, T> Values;
        private readonly IdentifierTokenReader Reader;

        public EnumTokenReader()
        {
            Values = Enum.GetValues<T>().ToDictionary(Value => Value.ToString());
            Reader = new IdentifierTokenReader();
        }

        public bool Read(ref TextSource Source, out IToken Token)
        {
            if (Reader.Read(ref Source, out IToken WordToken))
            {
                string Identifier = ((WordToken)WordToken).Value;

                if (Values.TryGetValue(Identifier, out T Value))
                {
                    Token = new EnumToken<T>()
                    {
                        Length = Identifier.Length,
                        Value = Value
                    };
                    return true;
                }
            }

            Token = default!;
            return false;
        }
    }
}