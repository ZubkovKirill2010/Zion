namespace Zion.STP
{
    public abstract class Block
    {
        protected readonly StringView String;

        public event Action<int>? Changed;
        public int Length
        {
            get => field;
            set
            {
                Changed?.Invoke(value - field);
                field = value;
            }
        }

        public Block(StringView String, int Length)
        {
            this.String = String;
            this.Length = Length;
        }


        public abstract bool IsValid(int Start);

        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
    }
}