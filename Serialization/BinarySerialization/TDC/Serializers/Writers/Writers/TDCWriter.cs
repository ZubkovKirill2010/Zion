namespace Zion.Serialization.TDC
{
    public sealed class TDCWriter : NamedTDCWriter
    {
        #region Constructors
        //public MainTDCWriter(Stream Stream) : base(Stream) { }

        #endregion

        #region PublicMethods
        public void Write<T>(T Value, ITDCPrimitiveSerializer<T>? Writer)
        {
            CheckNullable(Value);
        }

        public void Write<T>(T Value) where T : ITDCPrimitive<T>
        {
            CheckNullable(Value);
        }

        #endregion

        #region PrivateMethods
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