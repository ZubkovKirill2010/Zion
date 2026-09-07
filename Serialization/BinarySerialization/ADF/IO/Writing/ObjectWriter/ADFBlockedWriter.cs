namespace Zion.Serialization.ADF
{
    public sealed class ADFBlockedWriter : ADFObjectWriter
    {
        internal ADFBlockedWriter(BaseADFWriter Base, ArenaStream Stream)
            : base(Base, Stream)
        {
            Dispose();
        }

        protected override void OnWrited(string Name, in uint NameId, in uint FormatId)
        {
            throw new NotImplementedException();
        }
    }
}