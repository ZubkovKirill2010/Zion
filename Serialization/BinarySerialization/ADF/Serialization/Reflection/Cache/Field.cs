using System.Reflection;

namespace Zion.Serialization.ADF
{
    public readonly struct Field
    {
        private readonly FieldGetter Getter;

        public readonly string Name;


        public Field(FieldInfo Info)
        {
            ArgumentNullException.ThrowIfNull(Info);

            Getter = FieldGetter.Create(Info);
            Name = GetCleanName(Info.Name);
        }

        public Field(FieldGetter Getter, string Name)
        {
            this.Getter = Getter.NotNull();
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