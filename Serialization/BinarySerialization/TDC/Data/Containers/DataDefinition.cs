namespace Zion.Serialization.TDC
{
    public readonly struct DataDefinition : IBinarySerializable<DataDefinition>
    {
        public readonly ushort Id;
        public readonly ushort TypeId;
        
        public DataDefinition(ushort Id, ushort TypeId)
        {
            this.Id = Id;
            this.TypeId = TypeId;
        }


        public DataDefinition New<T>(TypeIdRegistry TypeIdTable, ushort Id)
        {
            return new DataDefinition
            (
                Id,
                TypeIdTable.NotNull().GetOrAdd<T>()
            );
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Id);
            Writer.Write(TypeId);
        }

        public static DataDefinition Read(BinaryReader Reader)
        {
            return new DataDefinition
            (
                Reader.ReadUInt16(),
                Reader.ReadUInt16()
            );
        }
    }
}