namespace Zion.Serialization.ADF
{
    public readonly struct Reference
    {
        //B0: 0 - New object, 1 - Object exists

        public readonly ulong Id;
        public readonly DataDefinition Definition;

        public Reference(ulong Id, DataDefinition Definition)
        {
            this.Id = Id;
            this.Definition = Definition;
        }

        public static ulong CreateNewReference(long ObjectOffset)
        {
            return ((ulong)ObjectOffset << 1) | 1UL;
        }
    }
}