namespace Zion.Serialization.ADF
{
    public readonly struct Parameter
    {
        public readonly uint NameId;
        public readonly uint FormatId;

        public Parameter(uint NameId, uint FormatId)
        {
            if (NameId == 0u)
            {
                throw new ArgumentNullException(nameof(NameId), "Null StringId");
            }

            this.NameId = NameId;
            this.FormatId = FormatId;
        }
    }
}