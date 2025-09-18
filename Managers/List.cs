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

        public static List<TOut> ConvertAll<TIn, TOut>(List<TIn> List, Func<TIn, TOut> Converter)
        {
            var Result = new List<TOut>();
            foreach (TIn Item in List)
            {
                Result.Add(Converter(Item));
            }
            return Result;
        }
    }
}