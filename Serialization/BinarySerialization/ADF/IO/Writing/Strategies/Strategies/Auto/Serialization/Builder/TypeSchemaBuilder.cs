using System.Reflection;
using System.Runtime.CompilerServices;

namespace Zion.Serialization.ADF
{
    public static class TypeSchemaBuilder
    {
        public static TypeSchema Create(Type Type)
        {
            //TODO
            return new(GetAllFields(Type), Type);
        }

        private static Field[] GetAllFields(Type Type)
        {
            const BindingFlags Flags = BindingFlags.Public
                                     | BindingFlags.NonPublic
                                     | BindingFlags.Instance
                                     | BindingFlags.DeclaredOnly;

            FieldInfo[] Fields = Type.GetFields(Flags);
            List<Field> Result = new(Fields.Length);

            foreach (FieldInfo Info in Fields)
            {
                if (Info.IsStatic)
                {
                    continue;
                }

                string Name = Info.Name;
                bool IsNotAutoField = Info.Name[0] != '<';

                foreach (var Attribute in Info.GetCustomAttributes())
                {
                    if (Attribute is ADFIgnoreAttribute ||
                        (IsNotAutoField && Attribute is CompilerGeneratedAttribute))
                    {
                        goto NextField;
                    }

                    if (Attribute is ADFNameAttribute NameAttribute)//TODO: Проверять имя на повтор.
                    {
                        Name = NameAttribute.Name;
                    }
                }

                Result.Add(new(Info, Name));

            NextField:;
            }

            return Result.ToArray();
        }
    }
}