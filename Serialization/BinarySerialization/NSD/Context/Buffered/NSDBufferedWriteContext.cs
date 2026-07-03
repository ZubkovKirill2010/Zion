namespace Zion.Serialization.NSD
{
    public sealed class NSDBufferedWriteContext : NSDWriteContext
    {
        private readonly MemoryStream Buffer;
        private readonly BinaryWriter BufferWriter;

        public NSDBufferedWriteContext(Stream Stream) : base(Stream)
        {
            Buffer = new MemoryStream();
            BufferWriter = new BinaryWriter(Buffer);
        }


        protected override void WritePrimitiveSafe<T>(T Value)
        {
            Write(() => Value.Write(BufferWriter));
        }

        protected override void WriteSafe<T>(T Value)
        {
            void WriteValue()
            {
                using NSDBufferedWriteContext Context = new NSDBufferedWriteContext(Buffer);
                Value.Write(Context);
            }

            Write(WriteValue);
        }

        protected override void WriteSafe<T>(T Value, IBinaryWriter<T>? ObjectWriter = null)
        {
            if (ObjectWriter is not null)
            {
                Write(() => ObjectWriter.Write(BufferWriter, Value));
            }
            else if (Value is IBinaryWritable Writable)
            {
                WritePrimitiveSafe((dynamic)Writable);
            }
            else
            {
                BinarySerializer.WriterNotFound<T>();
            }
        }

        protected override void Dispose(bool Disposing)
        {
            if (Disposing)
            {
                if (Buffer.Length > 0)
                {
                    FlushBuffer();
                }

                BufferWriter.Dispose();
                Buffer.Dispose();
            }
        }


        private void Write(Action Write)
        {
            long StartPosition = Buffer.Position;

            Buffer.Seek(4, SeekOrigin.Current);
            Write();

            long FinalPosition = Buffer.Position;
            long Size = FinalPosition - StartPosition - 4;

            Buffer.Position = StartPosition;
            BufferWriter.Write((uint)Size);
            Buffer.Position = FinalPosition;

            FlushBuffer();
        }

        private void FlushBuffer()
        {
            Buffer.Position = 0;
            Buffer.CopyTo(Stream);
            Buffer.SetLength(0);
        }
    }
}