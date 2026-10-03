namespace Zion.Serialization.ADF
{
    internal readonly struct TypeSchema<T>
    {
        private readonly Type    Type;
        private readonly Type[]  Generics;
        private readonly Field[] Fields;
        private readonly HashSet<string> UsedNames;//Optimize: Remove?

        internal TypeSchema(Type Type, Field[] Fields, HashSet<string> UsedNames)
        {
            this.Type = Type.NotNull();
            this.Fields = Fields.NotNull();
            this.UsedNames = UsedNames.NotNull();
            this.Generics = Type.GetGenericArguments();
        }
    }
}