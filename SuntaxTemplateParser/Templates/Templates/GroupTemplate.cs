namespace Zion.STP
{
    public sealed class GroupTemplate : Template
    {
        private readonly Template[] Templates;

        public GroupTemplate(params Template[] Templates)
        {
            this.Templates = Templates;
        }

        public override bool Read(StringView String, int Start, out Block Block)
        {
            Template[] Templates = this.Templates;
            Block[]    Blocks    = new Block[Templates.Length];
            int Length = 0;

            foreach (int Index in ZEnumerable.Range(Blocks))
            {
                if (Templates[Index].Read(String, Start, out Block CurrentBlock))
                {
                    Start  += CurrentBlock.Length;
                    Length += CurrentBlock.Length;

                    Blocks[Index] = CurrentBlock;
                }
                else
                {
                    Block = null;
                    return false;
                }
            }

            Block = new BlockGroup(Blocks, Length);
            return true;
        }
    }
}