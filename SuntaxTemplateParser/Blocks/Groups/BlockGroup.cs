namespace Zion.STP
{
    public sealed class BlockGroup : Group
    {
        private readonly Block[] Blocks;
        private int _Length;

        public override int Count => Blocks.Length;
        public override int Length => _Length;


        public BlockGroup(Block[] Blocks, int Length)
        {
            this.Blocks = Blocks;
            _Length = Length;
        }


        public override Block this[int Index] => Blocks[Index];


        public override bool Check(StringView String, int Start)
        {
            foreach (Block Block in Blocks)
            {
                if (Block.Check(String, Start))
                {
                    Start += Block.Length;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public override IEnumerator<Block> GetEnumerator()
        {
            foreach (Block Block in Blocks)
            {
                yield return Block;
            }
        }
    }
}