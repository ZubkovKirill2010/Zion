using System.Collections;

namespace Zion.STP
{
    public class Group : Block, IEnumerable<Block>
    {
        protected readonly List<Block> Blocks;
        protected readonly IGroupTemplate Template;

        public virtual int Count => Blocks.Count;

        public Group(StringView String, int Length, IGroupTemplate Template)
            : base(String, Length)
        {
            this.Template = Template;
            Blocks = new List<Block>();
        }


        public Block this[int Index]
        {
            get => Blocks[Index];
            set => Blocks[Index] = value;
        }

        public void Add(Block Block)
        {
            Length += Block.Length;
            Block.Changed += OnLengthChanged;
            Blocks.Add(Block);
        }


        public override bool IsValid(int Start)
        {
            if (Blocks.Count != Template.Count)
            {
                return false;
            }

            int Index = 0;

            while (Index < Blocks.Count)
            {
                if (Template[Index].IsMatch(String, Start, out Block Block) && Blocks[Index].Length == Block.Length)
                {
                    Start += Block.Length;
                }
                else
                {
                    return false;
                }
                Index++;
            }
            return true;
        }

        private void OnLengthChanged(int Additional)
        {
            Length += Additional;
        }


        public static Group GetEmpty(StringView String)
        {
            return new Group(String, new EmptyGroupTemplate());
        }


        public IEnumerator<Block> GetEnumerator()
        {
            return Blocks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}