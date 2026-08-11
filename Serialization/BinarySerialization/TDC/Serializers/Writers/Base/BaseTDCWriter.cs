namespace Zion.Serialization.TDC
{
    public abstract class BaseTDCWriter : IDisposable, IAsyncDisposable
    {
        #region Data
        protected readonly Stream Stream;

        protected readonly TypeIdRegistry TypeRegistry;
        #endregion

        #region Constructors
        public BaseTDCWriter(Stream Stream) : this(Stream, new()) { }

        public BaseTDCWriter(BaseTDCWriter Base) : this(Base.Stream, Base.TypeRegistry) { }

        public BaseTDCWriter(Stream Stream, TypeIdRegistry TypeRegistry)
        {
            ArgumentNullException.ThrowIfNull(Stream);
            if (!Stream.CanWrite)
            {
                throw new ArgumentException("Stream can not write", nameof(Stream));
            }

            this.Stream = Stream;
            this.TypeRegistry = TypeRegistry.NotNull();
            this.Primitives = Primitives.NotNull();
            this.Containers = Containers.NotNull();
        }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            Stream.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await Stream.DisposeAsync();
        }

        #endregion
    }
}