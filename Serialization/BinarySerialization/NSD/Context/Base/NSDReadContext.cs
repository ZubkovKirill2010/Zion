namespace Zion.Serialization.NSD
{
    public abstract class NSDReadContext
    {
        protected readonly Stream Stream;

        public NSDReadContext(Stream Stream)
        {
            ArgumentException.ThrowIf(!Stream.NotNull().CanRead, "The stream does not support reading");
            this.Stream = Stream;
        }


        public NSDBatchReader BeginRead()
        {
            return new NSDBatchReader(this);
        }

        internal protected abstract void ReadAll(NSDBatchReader Batch);
    }
}