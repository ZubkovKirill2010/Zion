namespace Zion.Serialization.TDC
{
    //Id -> Type
    public sealed class TypeRegistry
    {
        private readonly List<Type> Data;
        private int Writed;


        public TypeRegistry() : this(32) { }

        public TypeRegistry(int Capacity)
        {
            Data = new(Capacity);
        }


        public void Write(BinaryWriter Writer)
        {
            var Data = this.Data;
            int Count = Data.Count;
            int SimplePrimitiveCount = SimplePrimitive.Count;

            Writer.Write(Count - Writed);

            for (int i = Writed; i < Count; i++)
            {
                Writer.Write((ushort)(SimplePrimitiveCount + i));
                Writer.Write(Data[i].FullName.NotNull());
            }

            Writed = Count;
        }

        public void Read(BinaryReader Reader)
        {
            var Data = this.Data;
            int Count = Reader.ReadInt32();

            Data.EnsureCapacity(Data.Count + Count);

            for (int i = 0; i < Count; i++)
            {
                Data.Add(ReadType(Reader));
            }
        }


        private static Type ReadType(BinaryReader Reader)
        {
            string TypeName = Reader.ReadString();
            Type? Item = Type.GetType(TypeName);

            return Item ?? throw new ArgumentNullException($"Type '{TypeName}' not found"); ;
        }
    }
}