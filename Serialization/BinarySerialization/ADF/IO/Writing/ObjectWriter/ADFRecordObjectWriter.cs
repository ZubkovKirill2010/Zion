namespace Zion.Serialization.ADF
{
    public sealed class ADFRecordObjectWriter : ADFObjectWriter
    {
        private readonly Type Type;
        private readonly List<Parameter> Parameters;


        public ADFRecordObjectWriter(ADFWritingContext Context, StreamGroup Target, Type Type)
            : base(Context, Target)
        {
            this.Type = Type;
            Parameters = new();
        }


        protected override StreamGroup GetStreamGroup(string Name, in uint NameId, in uint FormatId)
        {
            ThrowIfContains(Name, in NameId);
            return base.GetStreamGroup(Name, in NameId, in FormatId);
        }

        protected override void OnWrited(string Name, in uint NameId, in uint FormatId)
        {
            Parameters.Add(new Parameter(NameId, FormatId));
        }


        public DataFormat BuildFormat()
        {
            Dispose();

            return DataFormatBuilder.Build(Parameters.ToArray(), Type, FormatRegistry, TypeAssociation);
        }

        
        private void ThrowIfContains(string Name, in uint NameId)
        {
            foreach (var Parameter in Parameters)
            {
                if (Parameter.NameId == NameId)
                {
                    throw new ADFRepeatedNameException(Name);
                }
            }
        }
    }
}