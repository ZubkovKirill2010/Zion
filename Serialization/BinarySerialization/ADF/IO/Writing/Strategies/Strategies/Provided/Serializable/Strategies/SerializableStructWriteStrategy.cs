namespace Zion.Serialization.ADF
{
    internal sealed class SerializableStructWriteStrategy<T> : StructWriteStrategy<T>
    {
        public SerializableStructWriteStrategy(ADFWritingContext Context)
            : base(Context)
        {
            if (!typeof(T).IsAssignableFrom(typeof(IADFWritable)))
            {
                throw new InvalidCastException($"{typeof(T)} not realized {typeof(IADFWritable)}");
            }
        }


        protected override void Write(ArenaStream BaseStream, ADFObjectWriter Writer, T Value)
        {
            var Writable = ((IADFWritable)Value!).NotNull();
            Writable.Write(Writer);
        }
    }
}