namespace Zion
{
    public sealed class Indexer<T>
    {
        public Func  <int, T>? Getter { private get; init; }
        public Action<int, T>? Setter { private get; init; }

        public bool ContainsGetter => Getter is not null;
        public bool ContainsSetter => Setter is not null;

        public T this[int Index]
        {
            get => Getter(Index);
            set => Setter(Index, value);
        }
    }
}