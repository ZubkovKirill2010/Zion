namespace Zion.Serialization.TDC
{
    public sealed class TDCWriter : NamedTDCWriter
    {
        #region Data
        private readonly Stream Stream;
        private readonly BinaryWriter StreamWriter;
        private readonly TDCWritingOptions Options;

        #endregion

        #region Constructors
        public TDCWriter(Stream Stream) : this(Stream, TDCWritingOptions.Default) { }

        public TDCWriter(Stream Stream, TDCWritingOptions Options)
        {
            ArgumentException.ThrowIf(!Stream.NotNull().CanWrite, "Stream can not Write");
            this.Stream = Stream;
            this.Options = Options.NotNull();

            StreamWriter = new BinaryWriter(Stream);

            if (Options.WriteHeader)
            {
                WriteHeader();
            }
        }

        #endregion

        #region PublicMethods
        public void Flush()
        {            
            var Memory = this.Memory;
            var Length = Memory.Length;

            if (Length == 0L) { return; }

            var Writer = StreamWriter;
            var Buffer = Memory.GetBuffer();

            TypeRegistry.Write(Writer);
            DataRegistry.Write(Writer);
            Stream.Write(Buffer, 0, (int)Length);

            Memory.SetLength(0L);
        }

        #endregion

        #region OverrideMethods
        protected override void OnWrited(string Key, ushort TypeId)
        {
            if (Memory.Length >= Options.MinPageSize)
            {
                Flush();
            }
        }

        protected override void OnDisposed()
        {
            Stream.WriteByte(0);
        }

        #endregion

        #region PrivateMethods
        private void WriteHeader()
        {
            //TODO: WriteHeader
        }


        private static bool IsNullable<T>()
        {
            return Nullable.GetUnderlyingType(typeof(T)) is not null;
        }

        private static void CheckNullable<T>(T Value)
        {
            if (Value is null && !IsNullable<T>())
            {
                string TypeName = typeof(T).Name;
                throw new ArgumentNullException
                (
                    nameof(Value),
                    $"Value cannot be null. If you need to store null values, use '{TypeName}?' (Nullable<{TypeName}>) instead."
                );
            }
        }

        #endregion
    }
}