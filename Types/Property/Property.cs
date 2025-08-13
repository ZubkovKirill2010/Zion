namespace Zion
{
    public sealed class Property<T>
    {
        private T Value;

        public Func<T, T> Getter { private get; init; } = Value => Value;
        public Func<T, T> Setter { private get; init; } = Value => Value;

        public Property()
        {
            Value = default;
        }
        public Property(T Value)
        {
            this.Value = Value;
        }
        public Property(T Value, Func<T, T> Getter, Func<T, T> Setter)
        {
            this.Value = Value;

            if (Getter is not null)
            {
                this.Getter = Getter;
            }
            if (Setter is not null)
            {
                this.Setter = Setter;
            }
        }

        public T Get() => Getter(Value);
        public void Set(T Value) => Value = Setter(Value);

        public bool IsNull() => Value is null;
    }
}