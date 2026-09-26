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
            Parameters.Add(new Parameter(NameId, 0u));
        }


        public DataFormat BuildFormat()
        {
            Dispose();

            return new DataFormat
            (
                Parameters.ToArray(),
                GetGenerics(),
                GetFlags(),
                GetBaseFormatId()
            );
        }


        private uint[] GetGenerics()
        {
            if (!Type.IsGenericType)
            {
                return [];
            }

            var Association = TypeAssociation;

            return Array.ConvertAll
            (
                Type.GetGenericArguments(),
                Association.GetOrAddDeferred
            );
        }

        private FormatFlags GetFlags()
        {
            var Type = this.Type;
            var Flags = FormatFlags.None;

            if (!Type.IsValueType)
            {
                Flags |= FormatFlags.IsReference;
            }
            if (Type.IsAbstract)
            {
                Flags |= FormatFlags.IsAbstract;
            }
            if (Type.IsNullable)
            {
                Flags |= FormatFlags.IsNullable;
            }

            return Flags;
        }

        private uint GetBaseFormatId()
        {
            var Base = Type.BaseType;
            return DataFormat.HasBase(Type)
                ? 0u
                : TypeAssociation.GetOrAddDeferred(Base);
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