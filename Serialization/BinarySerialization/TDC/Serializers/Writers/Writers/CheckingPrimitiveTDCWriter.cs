namespace Zion.Serialization.TDC
{
    public sealed class CheckingPrimitiveTDCWriter : PrimitiveTDCWriter
    {
        #region Data
        private readonly PrimitiveFormat Format;
        private int Current;

        #endregion

        #region Constructors
        public CheckingPrimitiveTDCWriter(BaseTDCWriter Base, PrimitiveFormat Format) : base(Base)
        {
            this.Format = Format;
        }

        #endregion

        #region OverrideMethods
        protected override void OnWrited(ushort TypeId)
        {
            if (Format[Current] != TypeId)
            {
                throw new InvalidOperationException
                (
                    $"Primitive format mismatch at index {Current}."
                );
            }
            Current++;
        }

        #endregion
    }
}