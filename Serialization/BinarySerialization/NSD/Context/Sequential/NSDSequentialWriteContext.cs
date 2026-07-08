namespace Zion.Serialization.NSD
{
    public sealed class NSDSequentialWriteContext : NSDWriteContext
    {
        public NSDSequentialWriteContext(Stream Stream) : base(Stream)
        {
            ArgumentException.ThrowIf(!Stream.CanSeek, "The stream does not support seeking");
        }


        protected override void WritePrimitiveSafe<T>(T Value)
        {
            Write(() => Writer.Write(Value));
        }

        protected override void WriteSafe<T>(T Value)
        {
            void WriteValue()
            {
                NSDSequentialWriteContext Context = new NSDSequentialWriteContext(Stream);
                Value.Write(Context);
            }
            Write(WriteValue);
        }

        protected override void WriteSafe<T>(T Value, IBinaryWriter<T>? ObjectWriter = null)
        {
            if (ObjectWriter is not null)
            {
                Write(() => ObjectWriter.Write(Writer, Value));
            }
            else if (Value is IBinaryWritable Writable)
            {
                WritePrimitiveSafe(Writable);
            }
            else
            {
                BinarySerializer.WriterNotFound<T>();
            }
        }


        private void Write(Action Write)
        {
            long SizePosition = Stream.Position;

            Stream.Seek(4, SeekOrigin.Current);
            Write();

            long FinalPosition = Stream.Position;
            long Size = FinalPosition - SizePosition - 4;

            Stream.Position = SizePosition;
            Writer.Write((uint)Size);
            Stream.Position = FinalPosition;
        }
    }
}