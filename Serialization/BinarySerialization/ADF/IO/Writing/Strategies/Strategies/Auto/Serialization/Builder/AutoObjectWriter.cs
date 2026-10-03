using System.Reflection;
using System.Runtime.CompilerServices;

namespace Zion.Serialization.ADF
{
    internal static class AutoObjectWriter
    {
        private static readonly MethodInfo WriteEnumMethod = typeof(AutoObjectWriter).GetMethod(nameof(WriteEnum))!;

        public static MethodInfo GetEnumWriter(Type EnumType)
        {
            return WriteEnumMethod.MakeGenericMethod(EnumType);
        }

        public static void WriteEnum<T>(ADFWritingContext Context, StreamGroup Target, T Value) where T : struct, Enum
        {
            Target.BaseStream.UseSpan
            (
                Unsafe.SizeOf<T>(),
                span => Unsafe.WriteUnaligned(ref span[0], Value)
            );
        }
    }
}