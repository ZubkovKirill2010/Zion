namespace Zion.Serialization.ADF
{
    public readonly struct Reference
    {
        public readonly uint Id;
        public readonly DataDefinition Definition;

        public Reference(uint Id, DataDefinition Definition)
        {
            this.Id = Id;
            this.Definition = Definition;
        }
    }
}