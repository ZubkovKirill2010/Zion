namespace Zion.STP
{
    public abstract class Node
    {
        public int TokensCount
        {
            get;
            init
            {
                ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
                field = value;
            }
        }

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


        public virtual Symbol? GetSymbol() => null;

        public virtual void Verificate(SemanticContext Context) { }

        public virtual void ApplyFormat(TokenSlice Tokens) { }
    }
}