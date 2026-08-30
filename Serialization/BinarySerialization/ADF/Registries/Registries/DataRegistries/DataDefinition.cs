namespace Zion.Serialization.ADF
{
    public readonly struct DataDefinition
    {
        public readonly uint FormatId;
        public readonly uint Page;
        public readonly int  Position;

        public DataDefinition(uint FormatId, uint Page, int Position)
        {
            this.FormatId = FormatId;
            this.Page     = Page;
            this.Position = Position;
        }
    }
}