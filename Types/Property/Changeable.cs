namespace Zion
{
    public sealed class Changeable<T>
    {
        public static readonly Func<T, T, bool> NotNull = static (Old, New) => New is not null;

        public T Value
        {
            get;
            set
            {
                if (CanSet(field, value))
                {
                    field = value;
                    Changed?.Invoke(value);
                }
            }
        }

        private readonly Func<T, T, bool> CanSet = static (Old, New) => true;

        public Changeable(T DefaultValue)
        {
            Value = DefaultValue;
        }
        public Changeable(T DefaultValue, Func<T, T, bool> CanSet)
        {
            this.CanSet ??= CanSet;
            Value = DefaultValue;
        }

        public event Action<T>? Changed;
    }
}