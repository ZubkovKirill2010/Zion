namespace Zion.STP
{
    public sealed class SemanticContext
    {
        public readonly TokenSlice Tokens;
        public readonly SemanticData Data;

        private readonly int Start;
        private readonly TokenErrorCollector ErrorCollector;

        public SemanticContext(TokenSlice Tokens, int Start, SemanticData Data, TokenErrorCollector ErrorCollector)
        {
            this.Tokens = Tokens;
            this.Start = Start;
            this.Data = Data.NotNull();
            this.ErrorCollector = ErrorCollector.NotNull();
        }

        public void AddError(int TokenPosition, short ErrorCode)
        {
            Token Token = Tokens[TokenPosition];
            if (Token.Status == Validation.Valid)
            {
                ErrorCollector.Add(Start + TokenPosition);
            }
            Token.AddError(ErrorCode);
        }
    }
}