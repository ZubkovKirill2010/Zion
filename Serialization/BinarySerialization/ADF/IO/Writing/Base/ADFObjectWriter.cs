namespace Zion.Serialization.ADF
{
    public abstract class ADFObjectWriter : BaseADFWriter
    {
        internal ADFObjectWriter(BaseADFWriter Base)
            : base(Base) { }

        internal ADFObjectWriter(BaseADFWriter Base, ArenaStream Stream)
            : base(Base, Stream) { }
    }
}