namespace Zion.Serialization.ADF
{
    public abstract class ADFObjectWriter : BaseADFWriter
    {
        internal ADFObjectWriter(ADFWritingContext Context, ArenaStream BaseStream)
            : base(Context, BaseStream) { }
    }
}