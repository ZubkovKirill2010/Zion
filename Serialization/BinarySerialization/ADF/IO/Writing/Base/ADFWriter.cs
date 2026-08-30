namespace Zion.Serialization.ADF
{
    public sealed class ADFWriter : BaseADFWriter
    {
        #region Data
        public readonly Stream BaseStream;
        private readonly BinaryWriter Writer;

        private bool IsFirstPage;

        private uint CurrentPage = 0;
        private int LastPosition = 0;

        #endregion

        #region Constructors
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

        #endregion

        #region PublicMethods
        public void Flush()
        {
            if (IsFirstPage)
            {
                WriteHeader();
                IsFirstPage = false;
            }
            WritePage();
        }

        #endregion

        #region OverrideMethods
        protected override void OnWrited(string Name, in uint NameId, in uint FormatId)
        {
            if (DataRegistry.Contains(NameId))
            {
                throw new ADFRepeatedNameException(Name);
            }

            DataDefinition Definition = new DataDefinition
            (
                FormatId,
                CurrentPage,
                LastPosition
            );
            LastPosition = CurrentPosition;
            DataRegistry.Add(Name, NameId, Definition);
        }

        protected override void OnDisposed()
        {
            Flush();
            Writer.Write(false);
        }

        #endregion

        #region PrivateMethods
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

        #endregion
    }
}