namespace Zion.Serialization.ADF
{
    public abstract class ADFObjectWriter : BaseADFWriter
    {
        internal ADFObjectWriter(BaseADFWriter Base)
            : base(Base) { }

        protected override void OnDisposed()
        {
            
        }
    }
}