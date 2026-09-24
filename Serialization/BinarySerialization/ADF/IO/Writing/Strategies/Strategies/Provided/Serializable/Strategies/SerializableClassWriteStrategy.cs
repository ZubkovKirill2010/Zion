namespace Zion.Serialization.ADF
{
    internal sealed class SerializableClassWriteStrategy<T> : ClassWriteStrategy<T>
    {
        public SerializableClassWriteStrategy(ADFWritingContext Context)
            : base(Context)
        {
            if (!typeof(T).IsAssignableFrom(typeof(IADFWritable)))
            {
                throw new InvalidCastException($"{typeof(T)} not realized {typeof(IADFWritable)}");
            }
        }


        protected override void Write(ADFObjectWriter Writer, T Value)
        {
            var Writable = ((IADFWritable)Value!).NotNull();
            Writable.Write(Writer);
        }
    }
}