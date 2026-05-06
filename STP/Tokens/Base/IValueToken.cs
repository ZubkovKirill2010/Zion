namespace Zion.STP
{
    public interface IValueToken<T> : IToken
    {
        public T Value { get; init; }
    }
}