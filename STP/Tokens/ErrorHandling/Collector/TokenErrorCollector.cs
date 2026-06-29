namespace Zion.STP
{
    public sealed class TokenErrorCollector
    {
        private readonly ListView<Token> Tokens;
        private readonly SortedList<int> Errors;

        public readonly ListView<int> ErrorsView;

        public TokenErrorCollector(ListView<Token> Tokens)
        {
            this.Tokens = Tokens.NotNull();
            this.Errors = new SortedList<int>();
            this.ErrorsView = new ListView<int>(Errors);
        }


        public void Add(int Position)
        {
            Errors.Add(Position);
        }

        public void AddIfInvalid(int Position)
        {
            if (Tokens[Position].Status == Validation.Invalid)
            {
                Errors.Add(Position);
            }
        }
    }
}