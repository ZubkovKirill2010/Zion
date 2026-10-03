using System.Reflection;

namespace Zion.Serialization.ADF
{
    public readonly struct PrimitiveInfo
    {
        public readonly int Size;
        public readonly MethodInfo WriteMethod;

        public PrimitiveInfo(int Size, MethodInfo WriteMethod)
        {
            this.Size = Size;
            this.WriteMethod = WriteMethod.NotNull();
        }

        public static PrimitiveInfo Create<T>(int Size, AutoWriter<T> WriteMethod)
        {
            return new(Size, WriteMethod.Method);
        }
    }
}