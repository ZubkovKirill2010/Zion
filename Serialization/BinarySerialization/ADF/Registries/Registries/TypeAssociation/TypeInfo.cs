using System.Buffers.Binary;

namespace Zion.Serialization.ADF
{
    public sealed class TypeInfo
    {
        public readonly uint FormatId;

        private readonly byte[] Generics = [];


        public TypeInfo(uint FormatId, byte[] Generics)
        {
            this.FormatId = FormatId;
            this.Generics = Generics.NotNull();
        }


        public ReadOnlySpan<byte> GetGenerics()
        {
            return Generics;
        }


        public static TypeInfo Create(TypeAssociation TypeAssociation, Type Type, uint FormatId, bool Compression)
        {
            return new TypeInfo
            (
                FormatId,
                GetGenerics
                (
                    TypeAssociation,
                    Type,
                    Compression
                )
            );
        }


        private static byte[] GetGenerics(TypeAssociation TypeAssociation, Type Type, bool Compression)
        {
            if (!Type.IsGenericType)
            {
                return [];
            }

            var GenericTypes = Type.GetGenericArguments();
            var Current = 0;

            if (Compression)
            {
                Span<byte> Generics = stackalloc byte[GenericTypes.Length * 5];

                foreach (var GenericType in GenericTypes)
                {
                    uint GenericId = TypeAssociation.GetOrAdd(GenericType);
                    Write7BitEncodedUInt(Generics.Slice(Current), GenericId, out int Writed);
                    Current += Writed;
                }

                return Generics.Slice(0, Current).ToArray();
            }
            else
            {
                byte[] Generics = new byte[GenericTypes.Length * 4];

                foreach (var GenericType in GenericTypes)
                {
                    uint GenericId = TypeAssociation.GetOrAdd(GenericType);
                    BinaryPrimitives.WriteUInt32LittleEndian(Generics.AsSpan(Current), GenericId);
                    Current += sizeof(uint);
                }

                return Generics;
            }
        }

        private static void Write7BitEncodedUInt(Span<byte> Span, uint Value, out int Writed)
        {
            var Index = 0;

            while (Value >= 0x80)
            {
                Span[Index++] = (byte)(Value | 0x80);
                Value >>= 7;
            }
            Span[Index++] = (byte)Value;

            Writed = Index;
        }
    }
}