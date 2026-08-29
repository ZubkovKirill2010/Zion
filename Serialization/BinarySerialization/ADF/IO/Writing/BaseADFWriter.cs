namespace Zion.Serialization.ADF
{
    public abstract class BaseADFWriter : IDisposable
    {
        #region Data
        private readonly Arena<byte> Arena;
        private readonly ArenaStream Stream;
        private readonly List<ADFObjectWriter> References;

        protected readonly ADFWritingOptions Options;
        protected readonly WritableRegistries Registries;

        protected StringIdRegistry StringRegistry => Registries.StringRegistry;
        protected FormatIdRegistry FormatRegistry => Registries.FormatRegistry;

        #endregion

        #region Properties
        public long TotalLength
        {
            get;
            private set
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                field = value;
            }
        } = -1L;

        public bool IsDisposed => TotalLength < 0;

        #endregion

        #region Constructors
        public BaseADFWriter(BaseADFWriter Base) : this(Base.Arena)
        {
            Registries = Base.Registries;
            Options = Base.Options;
        }

        public BaseADFWriter(Arena<byte> Arenas)
        {
            Arena = Arenas.NotNull();
            Stream = Arena.GetStream(64);
            References = new(0);
            Registries = new();
            Options = ADFWritingOptions.Default;
        }

        public BaseADFWriter(Arena<byte> Arenas, ADFWritingOptions? Options)
            : this(Arenas)
        {
            this.Options = Options ?? ADFWritingOptions.Default;
        }

        #endregion

        #region PublicMethods
        public void Flush(Stream Destination)
        {
            Stream.CopyTo(Destination);
            foreach (ADFObjectWriter Reference in References)
            {
                Reference.Flush(Destination);
            }
        }

        #endregion

        #region AbstractMethods
        protected abstract void OnDisposed();

        #endregion

        #region IDisposable
        public void Dispose()
        {
            long Length = Stream.Length;

            foreach (var Reference in References)
            {
                Length += Reference.TotalLength;
            }

            TotalLength = Length;

            OnDisposed();
        }

        #endregion

        #region PrivateMethods
        private void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(BaseADFWriter));
            }
        }

        #endregion
    }
}