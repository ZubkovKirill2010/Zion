namespace Zion.Serialization.ADF
{
    public sealed class ADFCheckingObjectWriter : ADFObjectWriter
    {
        private readonly DataFormat Format;
        private int Current;


        public ADFCheckingObjectWriter(BaseADFWriter Base, ArenaStream Stream, DataFormat Format)
            : base(Base, Stream)
        {
            this.Format = Format;
        }


        protected override void OnWrited(string Name, in uint NameId, in uint FormatId)
        {
            var Format = this.Format;
            var Parameter = Format[Current++];

            if (Current >= Format.ParametersCount)
            {
                throw new ADFTooManyParametersException(Current, Format.ParametersCount);
            }

            if (Parameter.NameId != NameId || !Format.Contains(NameId))
            {
                throw new ADFNameMismatchException(Name, StringRegistry.GetString(in NameId) ?? "null");
            }

            if (!FormatRegistry.IsAssignableFrom(Parameter.FormatId, FormatId))
            {
                throw new ADFFormatMismatchException(FormatId, Parameter.FormatId);
            }
        }

        protected override ArenaStream GetStream(in uint NameId)
        {
            //Если пишется объект, который идёт после ожидаемого, то мы его пишем в временный поток
        }
    }
}