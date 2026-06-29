namespace Zion.STP.Dynamic
{
    public interface ITokenContainer<TPointer> where TPointer : TextPointer<TPointer>
    {
        public int Count { get; }

        public void Add(Token Token);

        public void Overwrite(TPointer Start, int RemovedTokens, IEnumerable<Token> Tokens);

        public TPointer GetTokenStart(TPointer Position);

        public IEnumerable<Token> EnumeratorFrom(TPointer Start);
    }
}