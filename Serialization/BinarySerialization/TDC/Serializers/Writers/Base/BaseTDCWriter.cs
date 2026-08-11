namespace Zion.Serialization.TDC
{
    public abstract class BaseTDCWriter : IDisposable, IAsyncDisposable
    {
        #region Data
        protected readonly TypeIdRegistry TypeRegistry;
        protected readonly MemoryStream Memory;
        protected readonly BinaryWriter MemoryWriter;

        #endregion

        #region Constructors
        public BaseTDCWriter()
        {
            TypeRegistry = new();
            Memory = new();
            MemoryWriter = new(Memory);
        }

        public BaseTDCWriter(BaseTDCWriter Base)
        {
            TypeRegistry = Base.TypeRegistry;
            Memory       = Base.Memory;
            MemoryWriter = Base.MemoryWriter;
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