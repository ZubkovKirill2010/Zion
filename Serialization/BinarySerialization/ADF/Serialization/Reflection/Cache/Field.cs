using System.Reflection;

namespace Zion.Serialization.ADF
{
    public readonly struct Field
    {
        private readonly FieldGetter Getter;

        public readonly Type   Type;
        public readonly string Name;


        public Field(FieldInfo Info)
        {
            Getter = FieldGetter.Create(Info.NotNull());
            Type = Info.FieldType;
            Name = GetCleanName(Info.Name);
        }

        public Field(FieldInfo Info, string Name)
        {
            this.Getter = FieldGetter.Create(Info);
            this.Type = Info.FieldType;
            this.Name = Name.NotNull();
        }

        public Field(FieldGetter Getter, Type Type, string Name)
        {
            this.Getter = Getter.NotNull();
            this.Type = Type.NotNull();
            this.Name = GetCleanName(Name);
        }


        public object Get(object Source)
        {
            return Getter(Source);
        }


        internal static string GetCleanName(string Name)
        {
            ArgumentException.ThrowIfNullOrEmpty(Name);
            return Name[0] != '<'
                ? Name
                : Name[1..Name.IndexOf('>')];
        }
    }
}