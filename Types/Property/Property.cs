namespace Zion
{
    public sealed class Property<T>
    {
        private T Value;

        public Func<T, T> Getter { private get; init; } = Value => Value;
        public Func<T, T> Setter { private get; init; } = Value => Value;

        public bool ContainsGetter => Getter is not null;
        public bool ContainsSetter => Setter is not null;
        public bool IsNull => Value is null;

        public Property(T Value)
        {
            this.Value = Value;
        }

        public T Get() => Getter(Value);
        public void Set(T Value) => Value = Setter(Value);
    }
}