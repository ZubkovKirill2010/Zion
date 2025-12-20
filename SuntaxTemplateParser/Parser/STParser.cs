namespace Zion.STP
{
    public sealed class STParser
    {
        private StringView String;
        private List<Group> Groups;

        public int Length => String.Length;

        public Caret Caret;

        public STParser(StringView String)
        {
            this.String = String;
            Groups = new List<Group>(30)
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