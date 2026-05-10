namespace Zion.STP
{
    public abstract class Node
    {
        public int TokensCount { get; init; }
        public Validation Status
        {
            get;
            protected set
            {
                if (value != field)
                {
                    field = value;
                    StatusChanged?.Invoke(value);
                }
            }
        }

        public event Action<Validation>? StatusChanged;
    }
}