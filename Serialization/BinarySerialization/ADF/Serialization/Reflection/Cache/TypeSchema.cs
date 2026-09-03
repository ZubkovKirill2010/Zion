using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Zion.Serialization.ADF
{
    public readonly struct TypeSchema : IEnumerable<Field>
    {
        #region Data
        private readonly Field[] Fields;
        
        public readonly Type Type;
        public readonly DataFormat Format; //TODO: Create format

        #endregion

        #region Properties
        public int Count => Fields.Length;

        #endregion

        #region Constructors
        private TypeSchema(Field[] Fields, Type Type)
        {
            this.Fields = Fields;
            this.Type = Type;
        }

        #endregion

        #region Indexers
        public Field this[int   Index] => Fields[Index];
        
        public Field this[Index Index] => Fields[Index];

        #endregion

        #region PublicMethods
        public static TypeSchema Create(Type Type)
        {
            return new(GetAllFields(Type), Type);
        }

        #endregion

        #region IEnumerable
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<Field> GetEnumerator()
        {
            return Fields.Enumerate();
        }

        #endregion

        #region PrivateMethods
        private static Field[] GetAllFields(Type Type)
        {
            const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            FieldInfo[] Fields = Type.GetFields(Flags);
            List<Field> Result = new(Fields.Length);

            foreach (FieldInfo Info in Fields)
            {
                if (!Info.IsStatic && IsSerializableField(Info))
                {
                    Result.Add(new Field(Info));
                }
            }

            return Result.ToArray();
        }

        private static bool IsSerializableField(FieldInfo Field)
        {
            return !Field.IsDefined(typeof(ADFIgnoreAttribute), false)
               && (!Field.IsDefined(typeof(CompilerGeneratedAttribute), false)
               ||   Field.Name.Contains("k__BackingField"));
        }

        #endregion
    }
}