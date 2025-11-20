namespace Zion.STP
{
    public sealed class STParser
    {
        private StringView String;
        private List<Group> Groups;

        public int Length => String.Length;

        public int Caret
        {
            get => field;
            set
            {
                if (field == value)
                {
                    return;
                }
                if (IsInRange(value))
                {
                    field = value;
                    UpdateCurrent(value);
                }
                IndexOutOfRange(value);
            }
        }
        public Token? Current;

        public STParser(StringView String)
        {
            this.String = String;
            Groups = new List<Group>(30);
        }


        public void MoveCaret(int Offset)
        {
            Caret += Offset;
        }
        public void MoveRight()
        {

        }
        public void MoveLeft()
        {

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
            Caret = Index;
            Write(Char);
        }


        private void UpdateCurrent(int Target)
        {
            if (Length == 0)
            {
                Current = null;
            }

            int Block = 0;
            int End = 0;

            while (Block < String.Length)
            {
                //[test1][test2][test3]
                //              ^Target
                //              ^

                End += Block;

                if (Target < End)
                {
                    Current = Blocks[Block];
                }

                Block++;
            }

            Current = null;
        }

        private bool IsInRange(int Index)
        {
            return Index >= 0 && Index < String.Length;
        }

        private void IndexOutOfRange(int Index)
        {
            throw new ArgumentOutOfRangeException($"Caret set: Value(={Index}) out of range 0..{String.Length}");
        }
        private T IndexOutOfRange<T>(int Index)
        {
            throw new ArgumentOutOfRangeException($"Caret set: Value(={Index}) out of range 0..{String.Length}");
        }
    }
}