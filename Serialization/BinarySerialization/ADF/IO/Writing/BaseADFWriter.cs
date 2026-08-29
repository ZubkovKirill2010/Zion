namespace Zion.Serialization.ADF
{
    public abstract class BaseADFWriter : IDisposable
    {
        #region Data
        private readonly Arena<byte> Arena;
        private readonly ArenaStream Stream;
        private readonly List<ADFObjectWriter> References;

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
        public BaseADFWriter(Arena<byte> Arenas)
        {
            Arena  = Arenas;
            Stream = Arena.GetStream(64);
            References = new(0);
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

        #region Writing

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
        }

        #endregion
    }
}