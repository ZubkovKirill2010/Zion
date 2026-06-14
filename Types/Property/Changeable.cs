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
                if (ShouldUpdate(field, value))
                {
                    field = value;
                    Changed?.Invoke(value);
                }
            }
        }

        private readonly Func<T, T, bool> ShouldUpdate = static (Old, New) => true;

        public Changeable(T DefaultValue)
        {
            Value = DefaultValue;
        }
        public Changeable(T DefaultValue, Func<T, T, bool> ShouldUpdate)
        {
            this.ShouldUpdate = ShouldUpdate.NotNull();
            Value = DefaultValue;
        }

        public event Action<T>? Changed;
    }
}