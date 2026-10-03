namespace Zion.Serialization.ADF
{
    internal abstract class StructWriteStrategy<T> : ProvidedWriteStrategy<T>
    {
        public StructWriteStrategy(uint FormatId) : base(FormatId) { }

        protected sealed override StreamGroup GetGroupForData(ADFWritingContext Context, StreamGroup BaseGroup)
        {
            return BaseGroup;
        }
    }
}