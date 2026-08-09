namespace Zion.Serialization.TDC
{
    public sealed class PrimitiveTDCWriter : BaseTDCWriter
    {
        private readonly List<Type> Types;

        public PrimitiveTDCWriter(BaseTDCWriter Base)
            : base(Base)
        {
            Types = new(8);
        }
    }
}