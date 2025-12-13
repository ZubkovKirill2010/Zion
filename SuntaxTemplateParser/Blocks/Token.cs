namespace Zion.STP
{
    public abstract class Token : Block
    {
        private readonly ITokenTemplate Template;

        public Token(StringView String, int Length, ITokenTemplate Template)
            : base(String, Length)
        {
            this.Template = Template;
        }

        protected bool IsValid(int Start, Predicate<char> Condition)
        {
            return String.Range(Start, Length).All(char.IsWhiteSpace);
        }
    }
}