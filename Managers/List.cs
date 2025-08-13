namespace Zion
{
    public static class List
    {
        public static TOut[] Convert<TIn, TOut>(IList<TIn> List, Converter<TIn, TOut> Converter)
        {
            TOut[] Result = new TOut[List.Count];
            for (int i = 0; i < List.Count; i++)
            {
                Result[i] = Converter(List[i]);
            }
            return Result;
        }
    }
}