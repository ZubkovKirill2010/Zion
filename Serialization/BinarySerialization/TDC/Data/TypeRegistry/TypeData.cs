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
            this.Type   = Type.NotNull();
            this.Format = Format.NotNull();
        }

        public TypeData(Type Type, TypeInfo Info)
        {
            this.TypeId = Info.TypeId;
            this.Type   = Type;
            this.Format = Info.Format;
        }

        public TypeData(KeyValuePair<Type, TypeInfo> Pair)
        {
            TypeId = Pair.Value.TypeId;
            Type   = Pair.Key;
            Format = Pair.Value.Format;
        }


        public TypeInfo GetTypeInfo()
        {
            return new TypeInfo(TypeId, Format);
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
                ReadType(Reader),
                Reader.Read(IFormat.Serializer)
            );
        }


        private static Type ReadType(BinaryReader Reader)
        {
            string TypeName = Reader.ReadString();
            Type? Item = Type.GetType(TypeName);

            return Item ?? throw new ArgumentNullException($"Type '{TypeName}' not found");
        }
    }
}