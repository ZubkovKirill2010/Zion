namespace Zion.Serialization.ADF
{
    public sealed class FormatRegistry : IWritableRegistry
    {
        private readonly List<DataFormat> Formats;
        private int Added;

        public bool IsChanged => Added > 0;

        public int Count => Formats.Count;


        public FormatRegistry()
        {
            Formats = new(32);
        }


        public DataFormat this[uint Id] => Formats[GetIndex(Id)];


        public void Write(ADFObjectWriter Writer)
        {
            //TODO: IWritableRegistry.Write
        }


        public uint Add(DataFormat Format)
        {
            uint Id = (uint)Formats.Count;
            Formats.Add(Format);
            Added++;
            return Id;
        }

        public uint AddDeferred(uint Base)
        {
            return Add(DataFormat.GetDeferredFormat(Base));
        }

        public void Clarify(uint FormatId, DataFormat Format)
        {
            //TODO: Clarify
        }

        public bool IsAssignableFrom(in uint FormatId, in uint TargetFormatId)
        {
            if (TargetFormatId == 0u)
            {
                return true;
            }

            int BaseFormat = GetIndex(FormatId);
            int Target = GetIndex(TargetFormatId);
            
            if (BaseFormat < ADFPrimitives.Count || BaseFormat >= Count
                || Target < ADFPrimitives.Count || Target >= Count)
            {
                return false;
            }

            while (true)
            {
                if (BaseFormat == 0)
                {
                    return false;
                }
                if (BaseFormat == Target)
                {
                    return true;
                }

                BaseFormat = (int)Formats[BaseFormat].BaseFormat;
            }
        }

        public bool TryGetFormat(uint FormatId, out DataFormat Format)
        {
            int Index = GetIndex(FormatId);

            if (Index < 0 || Index >= Count)
            {
                Format = default;
                return false;
            }

            Format = Formats[Index];
            return true;
        }


        public IEnumerable<DataFormat> EnumerateHierarchy(DataFormat Low)
        {
            if (!IsRootFormat(Low))
            {
                foreach (var Hierarchy in EnumerateHierarchy(this[Low.BaseFormat]))
                {
                    yield return Hierarchy;
                }
            }

            yield return Low;
        }


        public static bool IsRootFormat(in DataFormat Format)
        {
            return Format.BaseFormat == 0;
        }


        private static int GetIndex(uint Id)
        {
            if (Id < ADFPrimitives.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(Id), $"Id(={Id}) out of range [{ADFPrimitives.Count}..)");
            }
            return (int)Id - ADFPrimitives.Count;
        }
    }
}