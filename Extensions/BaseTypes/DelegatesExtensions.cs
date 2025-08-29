namespace Zion
{
    public static class DelegatesExtensions
    {
        public static TryParser<T> TryParse<T>(this Converter<string, T?> Parser)
        {
            return (string String, out T Value) =>
            {
                T? Input = Parser(String);
                Value = Input ?? default;
                return Input is not null;
            };
        }
    }
}