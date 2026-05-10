namespace Zion.STP
{
    public abstract class Token
    {
        private static readonly short[] EmptyErrorCodes = Array.Empty<short>();

        public int Length
        {
            get;
            init
            {
                field = value > 0 ? value : throw new ArgumentOutOfRangeException($"Token.Length(={value}) <= 0");
            }
        }
        public short[] ErrorCodes
        {
            get;
            init
            {
                field = value.NotNull();
                Status = value.Length == 0 ? Validation.Valid : Validation.Invalid;
            }
        } = EmptyErrorCodes;

        public Validation Status { get; private init; }


        public override string ToString()
        {
            return $"[{GetType().Name.RemoveSuffix("Token")}]";
        }
    }
}