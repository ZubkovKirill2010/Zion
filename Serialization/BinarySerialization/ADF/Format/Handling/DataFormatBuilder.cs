namespace Zion.Serialization.ADF
{
    internal static class DataFormatBuilder
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

        public static DataFormat Build(TypeSchema Schema, ADFWritingContext Context)
        {
            var Type = Schema.Type;
            var FormatRegistry = Context.Registries.FormatRegistry;
            var StringRegistry = Context.Registries.StringRegistry;
            var TypeAssociation = Context.TypeAssociation;

            return new DataFormat
            (
                GetParameters(Schema.Fields, FormatRegistry, TypeAssociation, StringRegistry),
                GetGenerics(Schema.Generics, FormatRegistry, TypeAssociation),
                Schema.Flags,
                GetBaseFormatId(Type, FormatRegistry, TypeAssociation)
            );
        }


        public static FormatFlags GetFlags(Type Type)
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


        private static Parameter[] GetParameters(Field[] Fields, FormatRegistry FormatRegistry, TypeAssociation TypeAssociation, StringIdRegistry StringRegistry)
        {
            Parameter Convert(Field Field)
            {
                return new Parameter
                (
                    StringRegistry.GetOrAdd(Field.Name),
                    TypeAssociation.GetOrAddDeferred(Field.Type, FormatRegistry)
                );
            }

            return Array.ConvertAll(Fields, Convert);
        }

        private static uint[] GetGenerics(Type Type, FormatRegistry FormatRegistry, TypeAssociation TypeAssociation)
        {
            return Type.IsGenericType
                ? GetGenerics(Type.GetGenericArguments(), FormatRegistry, TypeAssociation)
                : [];
        }

        private static uint[] GetGenerics(Type[] Generics, FormatRegistry FormatRegistry, TypeAssociation TypeAssociation)
        {
            return Array.ConvertAll
            (
                Generics, Generic => TypeAssociation.GetOrAddDeferred(Generic, FormatRegistry)
            );
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