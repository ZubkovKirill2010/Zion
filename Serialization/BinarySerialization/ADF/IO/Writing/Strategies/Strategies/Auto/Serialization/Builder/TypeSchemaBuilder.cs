using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Zion.Serialization.ADF
{
    public static class TypeSchemaBuilder<T>
    {
        private const BindingFlags Flags = BindingFlags.Public
                                         | BindingFlags.NonPublic
                                         | BindingFlags.Instance
                                         | BindingFlags.DeclaredOnly;

        public static TypeSchema<T> Create(ADFWritingContext WritingContext, Type Type)
        {
            CheckContext(WritingContext, Type);

            var Fields = Type.GetFields(Flags);
            var Context = new TypeSchemaBuilderContext<T>(Fields.Length);
            var Writer = Expression.Lambda<AutoWriter<T>>
            (
                Expression.Block(GetWriters(Fields, Context)),
                Context.TargetParameter,
                Context.ValueParameter
            ).Compile();

            return new(Writer, Context.FieldNames);
        }


        private static IEnumerable<Expression> GetWriters(FieldInfo[] Fields, TypeSchemaBuilderContext<T> Context)
        {
            foreach (var Pair in FilterFields(Fields))
            {
                var Info = Pair.Item1;
                var Name = Pair.Item2;

                if (!Context.FieldNames.Add(Name))
                {
                    throw new ADFRepeatedNameException(Name);
                }

                yield return GetFieldWriter(Info, Context);
            }
        }

        private static IEnumerable<(FieldInfo, string)> FilterFields(FieldInfo[] Fields)
        {
            for (int i = 0; i < Fields.Length; i++)
            {
                var Info = Fields[i];
                var Name = Info.Name;
                var IsNotAutoField = !IsAutoField(ref Name);

                if (Info.FieldType.IsAssignableTo(typeof(Delegate)))
                {
                    continue;
                }

                foreach (var Attribute in Info.GetCustomAttributes())
                {
                    if (Attribute is ADFIgnoreAttribute or NonSerializedAttribute)
                    {
                        goto Continue;
                    }

                    if (IsNotAutoField && Attribute is CompilerGeneratedAttribute)
                    {
                        goto Continue;
                    }

                    if (Attribute is ADFNameAttribute NameAttribute)
                    {
                        Name = NameAttribute.Name;
                    }
                }

                yield return (Info, Name);

                Continue:;
            }
        }


        private static Expression GetFieldWriter<T>(FieldInfo Info, TypeSchemaBuilderContext<T> Context)
        {
            var FieldAccess = Expression.Field(Context.ValueParameter, Info);
            var BaseStream  = Expression.Property(Context.TargetParameter, nameof(StreamGroup.BaseStream));
            var WriteMethod = GetWriteMethod(Info.FieldType);

            return Expression.Call(BaseStream, WriteMethod, FieldAccess);
        }

        private static MethodInfo GetWriteMethod(Type Type)
        {
            //TODO: GetWriteMethod
        }


        private static bool IsAutoField(ref string Name)
        {
            if (Name[0] == '<' && Name.EndsWith(">k__BackingField"))
            {
                Name = Name[1..^16];
                return true;
            }
            return false;
        }

        private static void CheckContext(ADFWritingContext Context, Type Type)
        {
            ArgumentNullException.ThrowIfNull(Context);
            ArgumentNullException.ThrowIfNull(Type);
            if (!Type.IsAssignableTo(typeof(T)))
            {
                throw new InvalidCastException($"{Type} can not assignable to {typeof(T)}");
            }
        }
    }
}