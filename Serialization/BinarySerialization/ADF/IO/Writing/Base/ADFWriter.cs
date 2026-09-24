namespace Zion.Serialization.ADF
{
    public sealed class ADFWriter : BaseADFWriter
    {
        #region Data
        public  readonly Stream BaseStream;
        private readonly BinaryWriter Writer;

        private bool IsFirstPage;

        private uint CurrentPage = 0;
        private int LastPosition = 0;

        #endregion

        #region Constructors
        public ADFWriter(Stream Stream, ADFWritingOptions? Options = null) : base(new ADFWritingContext(Options))
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
        protected override StreamGroup GetStreamGroup(string Name, in uint NameId, in uint FormatId)
        {
            return DataRegistry.Contains(NameId)
                ? throw new ADFRepeatedNameException(Name)
                : base.GetStreamGroup(Name, in NameId, in FormatId);
        }

        protected override ArenaStream GetStreamForNull(string Name, in uint NameId)
        {
            return DataRegistry.Contains(NameId)
                ? throw new ADFRepeatedNameException(Name)
                : base.GetStreamForNull(Name, in NameId);
        }

        protected override void OnWrited(string Name, in uint NameId, in uint FormatId)
        {
            var Definition = new DataDefinition
            (
                FormatId,
                CurrentPage,
                LastPosition
            );

            LastPosition = CurrentPosition;
            DataRegistry.Add(Name, NameId, Definition);

            if (TotalLength >= Options.MinPageSize)
            {
                Flush();
            }
        }

        protected override void OnNullWrited(string Name, in uint NameId)
        {
            OnWrited(Name, in NameId, 0u);
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

            var Header = new ADFHeader()
            {
                Compression = Options.Compression
                //Other parameters in header
            };

            Header.Write(Writer);
        }

        private void WritePage()
        {
            Writer.Write(true);

            foreach (var Info in Registries)
            {
                var Registry = Info.Registry;

                if (Registry.IsChanged)
                {
                    Writer.Write(Info.Id);
                    
                    var Entry = WriteStrategies.GetEntry<IWritableRegistry>(Registry.GetType());
                    var Group = new StreamGroup(Context.Arena);

                    Entry.Strategy.Write(Group, Registry);
                }
            }

            Writer.Write((ushort)0);
            Writer.Write(TotalLength);

            Flush(BaseStream);

            Context.Arena.DisposeAll();
        }

        #endregion
    }
}