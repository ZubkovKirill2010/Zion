namespace Zion.Serialization.ADF
{
    public sealed class ADFRecordObjectWriter : ADFObjectWriter
    {
        private readonly Type Type;
        private readonly FormatFlags Flags;
        private readonly List<Parameter> Parameters;

        public DataFormat Format { get; private set; } = DataFormat.Object;


        public ADFRecordObjectWriter(BaseADFWriter Base, ArenaStream Stream, Type Type)
            : base(Base, Stream)
        {
            this.Type = Type;
            Flags = FormatFlags.FromType(Type);
            Parameters = new();
        }


        protected override void OnWrited(string Name, in uint NameId, in uint FormatId)
        {
            foreach (var Parameter in Parameters)
            {
                if (Parameter.NameId == NameId)
                {
                    throw new ADFRepeatedNameException(Name);
                }
            }
            Parameters.Add(new Parameter(NameId, FormatId));
        }

        protected override void OnDisposed()
        {
            Format = BuildFormat();
        }


        private DataFormat BuildFormat()
        {
            Type? BaseType = Type.BaseType;
            uint BaseFormat = 0u;

            if (BaseType is not null)
            {
                BaseFormat = TypeAssociation.GetOrAdd(BaseType);
            }

            return new DataFormat(Parameters, Flags, BaseFormat);
        }
    }
}