namespace Zion.Serialization.TDC
{
    public class TDCWriter : IDisposable, IAsyncDisposable
    {
        #region Data
        private readonly Stream Stream;

        #endregion

        #region Constructors
        public TDCWriter(Stream Stream)
        {
            ArgumentNullException.ThrowIfNull(Stream);
            if (!Stream.CanWrite)
            {
                throw new ArgumentException("Stream can not write", nameof(Stream));
            }

            this.Stream = Stream;
        }

        #endregion

        #region PublicMethods
        public void Write<T>(T Value)
        {
            ArgumentNullException.ThrowIfNull(Value);

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