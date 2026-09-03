namespace Zion.Serialization.ADF
{
    public static class FormatFlagsExtensions
    {
        extension(FormatFlags)
        {
            public static FormatFlags FromType(Type Type, bool IsDynamic = false)
            {
                ArgumentNullException.ThrowIfNull(Type);

                FormatFlags Flags = FormatFlags.None;

                if (Type.IsAbstract)
                    { Flags |= FormatFlags.IsAbstract; }

                if (Type.IsClass)
                    { Flags |= FormatFlags.IsReference; }

                if (!Type.IsNullable)
                    { Flags |= FormatFlags.IsNullable; }

                return Flags;
            }
        }
    }
}