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
                new EmptyGroup(String, 0, null)
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
            if (Length == 0)
            {
                String.Add(Char);
                return;
            }
        }

        public void Insert(int Index, char Char)
        {
            //Caret.Position = Index;
            Write(Char);
        }


        private bool IsInRange(int Index)
        {
            return Index.IsInRange(String);
        }
    }
}