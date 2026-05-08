namespace Zion
{
    public static class StringExtensions
    {
        extension(string String)
        {
            public static string FromCharCollection(ICollection<char> Collection)
            {
                if (Collection.NotNull().Count == 0)
                {
                    return string.Empty;
                }

                return string.Create(Collection.Count, Collection, (Span, Collection) =>
                {
                    int Index = 0;
                    foreach (char Char in Collection)
                    {
                        Span[Index++] = Char;
                    }
                });
            }
        }
    }
}