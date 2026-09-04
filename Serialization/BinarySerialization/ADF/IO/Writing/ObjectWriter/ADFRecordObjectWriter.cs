namespace Zion.Serialization.ADF
{
    public sealed class ADFRecordObjectWriter : ADFObjectWriter
    {
        public ADFRecordObjectWriter(BaseADFWriter Base)
            : base(Base) { }


        protected override ArenaStream GetStream(in uint NameId)
        {
            return base.GetStream(in NameId);
        }

        public DataFormat BuildFormat()
        {
            //TODO
        }
    }
}