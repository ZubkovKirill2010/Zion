namespace Zion.Serialization.TDC
{
    public readonly struct TypeInfo : IBinarySerializable<TypeInfo>
    {
        public readonly ushort TypeId;
        public readonly IFormat Format;


        public TypeInfo(ushort TypeId, IFormat Format)
        {
            this.TypeId = TypeId;
            this.Format = Format.NotNull();
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(TypeId);
            Writer.Write(Format, IFormat.Serializer);
        }

        public static TypeInfo Read(BinaryReader Reader)
        {
            return new TypeInfo
            (
                Reader.ReadUInt16(),
                Reader.Read(IFormat.Serializer)
            );
        }
    }
}