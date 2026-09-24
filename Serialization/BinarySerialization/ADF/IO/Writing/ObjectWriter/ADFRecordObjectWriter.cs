namespace Zion.Serialization.ADF
{
    public sealed class ADFRecordObjectWriter : ADFObjectWriter
    {
        private readonly FormatFlags Flags;
        private readonly List<Parameter> Parameters;

        private bool IsDeferred;


        public ADFRecordObjectWriter(ADFWritingContext Context, StreamGroup Target, Type Type)
            : base(Context, Target)
        {
            Flags = FormatFlags.FromType(Type);//TODO: Generics...
            Parameters = new();
        }


        protected override StreamGroup GetStreamGroup(string Name, in uint NameId, in uint FormatId)
        {
            ThrowIfContains(Name, in NameId);
            return base.GetStreamGroup(Name, in NameId, in FormatId);
        }

        protected override ArenaStream GetStreamForNull(string Name, in uint NameId)
        {
            ThrowIfContains(Name, in NameId);
            return base.GetStreamForNull(Name, NameId);
        }

        protected override void OnWrited(string Name, in uint NameId, in uint FormatId)
        {
            Parameters.Add(new Parameter(NameId, FormatId));
        }

        protected override void OnNullWrited(string Name, in uint NameId)
        {
            IsDeferred = true;
            Parameters.Add(new Parameter(NameId, 0u));
        }


        public DataFormat BuildFormat()
        {
            Dispose();
            //TODO: ADFRecordWriter.CreateFormat
            return new DataFormat(Parameters, Flags, 0u);
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