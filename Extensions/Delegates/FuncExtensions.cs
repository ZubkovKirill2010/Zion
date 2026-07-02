namespace Zion
{
    public static class FuncExtensionsions
    {
        public static bool Try<T>(this Func<T> Function, out T Result)
        {
            try
            {
                Result = Function();
                return true;
            }
            catch
            {
                Result = default!;
                return false;
            }
        }

        public static bool Try<TIn, TOut>(this Func<TIn, TOut> Function, TIn In, out TOut Result)
        {
            try
            {
                Result = Function(In);
                return true;
            }
            catch
            {
                Result = default!;
                return false;
            }
        }
    }
}