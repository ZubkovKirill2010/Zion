namespace Zion.Serialization.ADF
{
    public sealed class FormatIdRegistry : IWritableRegistry
    {
        public int NewItemsCount { get; private set; }


        public FormatIdRegistry()
        {

        }


        public DataFormat this[uint Id] { get; }//TODO


        public uint Add(DataFormat Format)
        {
            //TODO
        }        

        public bool IsAssignableFrom(in uint FormatId, in uint TargetFormatId)
        {
            //TODO
        }
    }
}