namespace Zion.Serialization.ADF
{
    public static class FormatFlagsExtensions
    {
        extension(FormatFlags)
        {
            public static FormatFlags FromType(Type Type)
            {
                ArgumentNullException.ThrowIfNull(Type);

                FormatFlags Flags = FormatFlags.None;
                
                if (Type.IsArray)
                {
                    Flags |= FormatFlags.IsArray;
                }

                if (!Type.IsValueType)
                {
                    Flags |= FormatFlags.IsReference;
                }

                if (Type.IsAbstract)
                {
                    Flags |= FormatFlags.IsAbstract;
                }

                if (Type.IsNullable)
                {
                    Flags |= FormatFlags.IsNullable;
                }

                if (Type.IsEnum)
                {
                    Flags |= FormatFlags.IsEnum;
                }

                return Flags;
            }
        }
    }
}