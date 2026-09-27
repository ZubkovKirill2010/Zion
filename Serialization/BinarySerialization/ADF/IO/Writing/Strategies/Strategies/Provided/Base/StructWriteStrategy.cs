namespace Zion.Serialization.ADF
{
    internal abstract class StructWriteStrategy<T> : ProvidedWriteStrategy<T>
    {
        public StructWriteStrategy(ADFWritingContext Context, uint FormatId)
            : base(Context, FormatId) { }


        protected sealed override StreamGroup GetGroupForData(StreamGroup BaseGroup)
        {
            return BaseGroup;
        }
    }
}