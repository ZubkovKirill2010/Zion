namespace Zion.Serialization.ADF
{
    internal abstract class StructWriteStrategy<T> : ProvidedWriteStrategy<T>
    {
        public StructWriteStrategy(ADFWritingContext Context)
            : base(Context) { }


        protected sealed override StreamGroup GetGroupForData(StreamGroup BaseGroup)
        {
            return BaseGroup;
        }
    }
}