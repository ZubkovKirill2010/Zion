namespace Zion.Serialization.ADF
{
    public sealed class ADFRecordObjectWriter : ADFObjectWriter
    {
        private readonly FormatFlags Flags;
        private readonly List<Parameter> Parameters;

        public DataFormat Format { get; private set; }


        public ADFRecordObjectWriter(BaseADFWriter Base, ArenaStream Stream, Type Type)
            : base(Base, Stream)
        {
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
            //TODO: Для классов реализовать систему наследования
            return new DataFormat(Parameters, Flags, 0u);
        }
    }
}