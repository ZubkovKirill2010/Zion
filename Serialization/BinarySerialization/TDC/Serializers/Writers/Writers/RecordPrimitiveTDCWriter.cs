namespace Zion.Serialization.TDC
{
    public sealed class RecordPrimitiveTDCWriter : PrimitiveTDCWriter
    {
        #region Data
        private readonly List<ushort> Format;

        #endregion

        #region Constructors
        public RecordPrimitiveTDCWriter(BaseTDCWriter Base) : base(Base)
        {
            Format = new();
        }

        #endregion

        #region PublicMethods
        public PrimitiveFormat GetFormat()
        {
            return new PrimitiveFormat(Format);
        }

        #endregion

        #region OverrideMethods
        protected override void OnWrited(ushort TypeId)
        {
            Format.Add(TypeId);
        }

        #endregion
    }
}