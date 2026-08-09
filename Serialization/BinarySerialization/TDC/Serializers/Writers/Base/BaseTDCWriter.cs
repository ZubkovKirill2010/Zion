namespace Zion.Serialization.TDC
{
    public abstract class BaseTDCWriter : IDisposable, IAsyncDisposable
    {
        #region Data
        protected readonly Stream Stream;

        protected readonly TypeIdRegistry TypeRegistry;
        protected readonly PrimitivesRegistry Primitives;
        protected readonly ContainersRegistry Containers;
        #endregion

        #region Constructors
        public BaseTDCWriter(Stream Stream) : this(Stream, new(), new(), new()) { }

        public BaseTDCWriter(BaseTDCWriter Base) : this(Base.Stream, Base.TypeRegistry, Base.Primitives, Base.Containers) { }

        public BaseTDCWriter(Stream Stream, TypeIdRegistry TypeRegistry, PrimitivesRegistry Primitives, ContainersRegistry Contaienrs)
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