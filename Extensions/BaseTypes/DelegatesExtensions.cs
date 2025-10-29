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

        public static AsyncAction ToAsync(Action Action)
        {
            return () => Task.Run(Action);
        }
        public static AsyncAction<T> ToAsync<T>(this Action<T> action)
        {
            return T => Task.Run(() => action(T));
        }
        public static AsyncAction<T1, T2> ToAsync<T1, T2>(this Action<T1, T2> action)
        {
            return (T1, T2) => Task.Run(() => action(T1, T2));
        }
        public static AsyncAction<T1, T2, T3> ToAsync<T1, T2, T3>(this Action<T1, T2, T3> action)
        {
            return (T1, T2, T3) => Task.Run(() => action(T1, T2, T3));
        }
        public static AsyncAction<T1, T2, T3, T4> ToAsync<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action)
        {
            return (T1, T2, T3, T4) => Task.Run(() => action(T1, T2, T3, T4));
        }
        public static AsyncAction<T1, T2, T3, T4, T5> ToAsync<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action)
        {
            return (T1, T2, T3, T4, T5) => Task.Run(() => action(T1, T2, T3, T4, T5));
        }
        public static AsyncAction<T1, T2, T3, T4, T5, T6> ToAsync<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action)
        {
            return (T1, T2, T3, T4, T5, T6) => Task.Run(() => action(T1, T2, T3, T4, T5, T6));
        }
    }
}