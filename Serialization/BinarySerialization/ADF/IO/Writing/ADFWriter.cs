namespace Zion.Serialization.ADF
{
    public sealed class ADFWriter : BaseADFWriter
    {
        public readonly Stream BaseStream;

        private readonly BinaryWriter Writer;

        private bool IsFirstPage;


        public ADFWriter(Stream Stream, ADFWritingOptions? Options = null)
            : base(new Arena<byte>(2048), Options)
        {
            if (!Stream.NotNull().CanWrite)
            {
                throw new InvalidOperationException("Stream can not write");
            }
            BaseStream = Stream;
            Writer = new(Stream);
            IsFirstPage = true;
        }


        protected override void OnDisposed()
        {
            Flush();
            Writer.Write(false);
        }


        public void Flush()
        {
            if (IsFirstPage)
            {
                WriteHeader();
                IsFirstPage = false;
            }
            WritePage();
        }


        private void WriteHeader()
        {
            if (!Options.WriteHeader) { return; }

            //TODO: Write header
        }

        private void WritePage()
        {
            Writer.Write(true);

            foreach (var Registry in Registries)
            {
                if (Registry.Registry.NewItemsCount > 0)
                {
                    Writer.Write(Registry.Id);
                    //TODO: Write registry
                }
            }

            Writer.Write((ushort)0);
            Writer.Write(TotalLength);

            Flush(BaseStream);
        }
    }
}