namespace Zion.Serialization.ADF
{
    public sealed class FormatIdRegistry : IWritableRegistry
    {
        private readonly List<DataFormat> Formats;

        public int NewItemsCount { get; private set; }

        public int Count => Formats.Count;


        public FormatIdRegistry()
        {
            Formats = new(32);
        }


        public DataFormat this[uint Id] => Formats[GetIndex(Id)];


        public uint Add(DataFormat Format)
        {
            uint Id = (uint)Formats.Count;
            Formats.Add(Format);
            NewItemsCount++;
            return Id;
        }        

        public bool IsAssignableFrom(in uint FormatId, in uint TargetFormatId)
        {
            if (TargetFormatId == 0u)
            {
                return true;
            }

            int BaseFormat = GetIndex(FormatId);
            int Target = GetIndex(TargetFormatId);
            
            if (BaseFormat < ADFPrimitives.PrimitiveCount || BaseFormat >= Count
                || Target < ADFPrimitives.PrimitiveCount || Target >= Count)
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


        private static int GetIndex(uint Id)
        {
            if (Id < ADFPrimitives.PrimitiveCount)
            {
                throw new ArgumentOutOfRangeException(nameof(Id), $"Id(={Id}) out of range [{ADFPrimitives.PrimitiveCount}..)");
            }
            return (int)Id - ADFPrimitives.PrimitiveCount;
        }
    }
}