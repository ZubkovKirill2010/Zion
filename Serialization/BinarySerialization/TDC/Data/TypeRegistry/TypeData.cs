namespace Zion.Serialization.TDC
{
    public readonly struct TypeData : IBinarySerializable<TypeData>
    {
        public readonly ushort TypeId;
        public readonly Type Type;
        public readonly IFormat Format;


        public TypeData(ushort TypeId, Type Type, IFormat Format)
        {
            this.TypeId = TypeId;
            this.Type = Type.NotNull();
            this.Format = Format.NotNull();
        }

        public TypeData(KeyValuePair<Type, TypeInfo> Pair)
        {
            TypeId = Pair.Value.TypeId;
            Type = Pair.Key;
            Format = Pair.Value.Format;
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(TypeId);
            Writer.Write(Type.FullName.NotNull());
            Writer.Write(Format, IFormat.Serializer);
        }

        public static TypeData Read(BinaryReader Reader)
        {
            return new TypeData
            (
                Reader.ReadUInt16(),
                TypeRegistry.ReadType(Reader),
                Reader.Read(IFormat.Serializer)
            );
        }
    }
}