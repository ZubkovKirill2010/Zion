namespace Zion.Serialization.ADF
{
    internal abstract class StructWriteStrategy<T> : ProvidedWriteStrategy<T>
    {
        public StructWriteStrategy(ADFWritingContext Context)
            : base(Context) { }


        protected sealed override ArenaStream GetStreamForData(StreamGroup BaseGroup)
        {
            return BaseGroup.BaseStream;
        }
    }
}