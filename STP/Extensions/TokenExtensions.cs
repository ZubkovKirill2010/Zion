namespace Zion.STP
{
    public static class TokenExtensions
    {
        extension<T>(T Token) where T : Token, new()
        {
            public static bool TryCreate(int Length, out Token Result)
            {
                if (Length > 0)
                {
                    Result = new T() { Length = Length };
                    return true;
                }

                Result = default!;
                return false;
            }
        }

        extension<T>(T Token) where T : Token
        {
            public static bool TryCreate(int Length, Func<T> Create, out Token Result)
            {
                if (Length > 0)
                {
                    Result = Create();
                    return true;
                }

                Result = default!;
                return false;
            }
        }
    }
}