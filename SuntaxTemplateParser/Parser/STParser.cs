namespace Zion.STP
{
    public sealed class STParser
    {
        private StringView  String;
        private List<Group> Groups;
        private Template[]  Templates;

        public int Length => String.Length;

        public Caret Caret;

        public STParser(StringView String, Template[] Templates, int Capacity = 40)
        {
            this.String = String;
            this.Templates = ZArray.Clone(Templates);
            Groups = new List<Group>(Math.Max(10, Capacity))
            {
                new BlockGroup(Array.Empty<Block>(), 0)
            };
            Caret = new Caret(String, Groups);
        }


        public void MoveCaret(int Offset)
        {
            Caret.Move(Offset);
        }

        public void MoveRight()
        {
            MoveCaret(1);
        }

        public void MoveLeft()
        {
            MoveCaret(1);
        }


        public void Write(char Char)
        {
            String.Insert(Caret.Position, Char);

            if (Caret.IsStart())
            {

            }
        }

        public void Insert(int Index, char Char)
        {
            Caret.SetPosition(Index);
            Write(Char);
        }
    }
}