namespace Zion.Serialization.TDC
{
    public class TDCReader : IDisposable, IAsyncDisposable
    {
        #region Data
        private readonly Stream Stream;

        private readonly TypeRegistry       TypeRegistry = new();
        private readonly DataRegistry       DataRegistry = new();
        #endregion

        #region Constructors
        public TDCReader(Stream Stream)
        {
            ArgumentNullException.ThrowIfNull(Stream);
            if (!Stream.CanRead)
            {
                throw new ArgumentException("Stream can not read", nameof(Stream));
            }

            this.Stream = Stream;
        }

        #endregion

        #region PublicMethods

        #endregion

        #region PrivateMethods

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