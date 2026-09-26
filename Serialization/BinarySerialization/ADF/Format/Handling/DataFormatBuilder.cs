namespace Zion.Serialization.ADF
{
    public static class DataFormatBuilder
    {
        public static DataFormat BuildDeferred(Type Type, FormatRegistry FormatRegistry, TypeAssociation TypeAssociation)
        {
            return Build([], Type, FormatRegistry, TypeAssociation);
        }

        public static DataFormat Build(Parameter[] Parameters, Type Type, FormatRegistry FormatRegistry, TypeAssociation TypeAssociation)
        {
            return new DataFormat
            (
                Parameters,
                GetGenerics(Type, FormatRegistry, TypeAssociation),
                GetFlags(Type),
                GetBaseFormatId(Type, FormatRegistry, TypeAssociation)
            );
        }


        private static uint[] GetGenerics(Type Type, FormatRegistry FormatRegistry, TypeAssociation TypeAssociation)
        {
            if (!Type.IsGenericType)
            {
                return [];
            }

            var Association = TypeAssociation;
            var Formats = FormatRegistry;
            var GenericTypes = Type.GetGenericArguments();
            var Generics = new uint[GenericTypes.Length];

            for (int i = 0; i < Generics.Length; i++)
            {
                var GenericType = GenericTypes[i];

                if (TypeAssociation.TryGetFormatId(GenericType, out var Existing))
                {
                    Generics[i] = Existing;
                }
                else
                {
                    var Id = Formats.AddDeferred();
                    TypeAssociation.Add(Type, Id);
                    Generics[i] = Id;
                }
            }

            return Generics;
        }

        private static FormatFlags GetFlags(Type Type)
        {
            var Flags = FormatFlags.None;

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

            return Flags;
        }

        private static uint GetBaseFormatId(Type Type, FormatRegistry FormatRegistry, TypeAssociation TypeAssociation)
        {
            var Base = Type.BaseType;
            return DataFormat.HasBase(Type)
                ? TypeAssociation.GetOrAddDeferred(Base!, FormatRegistry)
                : ADFPrimitives.Object;
        }
    }
}