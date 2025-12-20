namespace Zion.STP
{
    public sealed class CollectionTemplate : GroupTemplate
    {
        private readonly Template[] Templates;

        public CollectionTemplate(params Template[] Templates)
        {
            this.Templates = Templates;
        }

        public override bool Read(StringView String, int Start, out Group Group)
        {
            Template[] Templates = this.Templates;
            Block[] Blocks = new Block[Templates.Length];
            int Length = 0;

            foreach (int Index in ZEnumerable.Range(Blocks))
            {
                if (Templates[Index].Read(String, Start, out Block CurrentBlock))
                {
                    Start += CurrentBlock.Length;
                    Length += CurrentBlock.Length;

                    Blocks[Index] = CurrentBlock;
                }
                else
                {
                    Group = null;
                    return false;
                }
            }

            Group = new BlockGroup(Blocks, Length);
            return true;
        }
    }
}