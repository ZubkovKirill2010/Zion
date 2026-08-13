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
            
            if (Options.WriteHeader)
            {
                WriteHeader();
            }
        }

        #endregion

        #region PublicMethods
        public void Flush()
        {
            var Writer = StreamWriter;
            TypeRegistry.Write(StreamWriter);
            DataRegistry.Write(StreamWriter);

            var Memory = this.Memory;
            var Buffer = Memory.GetBuffer();
            var Length = (int)Memory.Length;
            Stream.Write(Buffer, 0, Length);

            Memory.SetLength(0L);
        }


        public void Write<T>(T Value, ITDCPrimitiveSerializer<T>? Writer)
        {
            CheckNullable(Value);
        }

        public void Write<T>(T Value) where T : ITDCPrimitive<T>
        {
            CheckNullable(Value);
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