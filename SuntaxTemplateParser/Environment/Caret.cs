namespace Zion.STP
{
    public sealed class Caret
    {
        private readonly object Lock = new object();

        private readonly StringView String;
        private readonly List<Group> Groups;
        private readonly Stack<(Group, int)> Stack; //(Parent, ChildIndex)

        public Group CurrentGroup { get; private set; }
        public Block CurrentBlock { get; private set; }

        public int Position { get; private set; }
        public int LocalPosition { get; private set; }

        public int Group
        {
            get => field;
            private set
            {
                field = value;
                CurrentGroup = Groups[value];
            }
        }
        public int Block
        {
            get => field;
            private set
            {
                field = value;
                CurrentBlock = CurrentGroup[value];
            }
        }


        public Caret(StringView String, List<Group> Groups)
        {
            ArgumentNullException.ThrowIfNull(String);
            ArgumentNullException.ThrowIfNull(Groups);

            this.String = String;
            this.Groups = Groups;
            Stack = new Stack<(Group, int)>();

            if (Groups.Count > 0)
            {
                Group = 0;
                if (CurrentGroup.Count > 0)
                {
                    Block = 0;
                    Position = 0;
                    LocalPosition = 0;
                }
            }
        }


        public void Move(int Offset)
        {
            if (Offset == 0) { return; }
            ArgumentOutOfRangeException.ThrowIfWithout(Position + Offset, String);

            if (int.IsPositive(Offset))
            {
                MoveRight(Offset);
            }
            else
            {
                MoveLeft(-Offset);
            }
        }

        public void SetPosition(int NewPosition)
        {
            Move(NewPosition - Position);
        }


        private void MoveLeft(int Offset)
        {
            int Remaining = Offset;
            int Index = Position;
            int Target = Index - Offset;

            while (Index > Target)
            {
                if (LocalPosition > 0)
                {
                    int Move = Math.Min(LocalPosition, Index - Target);

                    LocalPosition -= Move;
                    Position -= Move;
                    Index -= Move;
                    Remaining -= Move;
                }
                else
                {
                    PreviousBlock();
                    if (CurrentBlock is null)
                    {
                        break;
                    }

                    LocalPosition = CurrentBlock.Length - 1;
                    Position--;
                    Index--;
                    Remaining--;

                    while (CurrentBlock is Group Group)
                    {
                        Stack.Push((CurrentGroup, Block));
                        CurrentGroup = Group;
                        if (Group.Count == 0)
                        {
                            break;
                        }

                        Block = Group.Count - 1;
                        LocalPosition = CurrentBlock.Length - 1;
                    }
                }
            }
        }

        private void MoveRight(int Offset)
        {
            int Remaining = Offset;
            int Index = Position;
            int Target = Index + Offset;

            while (Index < Target)
            {
                if (CurrentBlock is null)
                {
                    break;
                }

                if (LocalPosition < CurrentBlock.Length - 1)
                {
                    int Move = Math.Min(CurrentBlock.Length - 1 - LocalPosition, Target - Index);

                    LocalPosition += Move;
                    Position += Move;
                    Index += Move;
                    Remaining -= Move;
                }
                else
                {
                    NextBlock();
                    if (CurrentBlock is null)
                    {
                        break;
                    }

                    LocalPosition = 0;
                    Position++;
                    Index++;
                    Remaining--;

                    while (CurrentBlock is Group Group)
                    {
                        Stack.Push((CurrentGroup, Block));
                        CurrentGroup = Group;

                        if (Group.Count == 0)
                        {
                            break;
                        }

                        Block = 0;
                        LocalPosition = 0;
                    }
                }
            }
        }


        private void NextBlock()
        {
            if (Block + 1 < CurrentGroup.Count)
            {
                Block++;
                return;
            }

            while (Stack.Count > 0)
            {
                (Group? ParentGroup, int ChildIndex) = Stack.Pop();
                CurrentGroup = ParentGroup;
                Block = ChildIndex;

                if (Block + 1 < CurrentGroup.Count)
                {
                    Block++;
                    return;
                }
            }

            if (Group + 1 < Groups.Count)
            {
                Group++;
                if (CurrentGroup.Count > 0)
                {
                    Block = 0;
                    return;
                }
            }

            CurrentBlock = null;
        }

        private void PreviousBlock()
        {
            if (Block > 0)
            {
                Block--;
                return;
            }

            if (Stack.Count > 0)
            {
                (Group? parentGroup, int childIndex) = Stack.Pop();
                CurrentGroup = parentGroup;
                Block = childIndex;
                return;
            }

            if (Group > 0)
            {
                Group--;
                if (CurrentGroup.Count > 0)
                {
                    Block = CurrentGroup.Count - 1;
                    return;
                }
            }

            CurrentBlock = null;
        }


        public bool IsEdge()
        {
            return LocalPosition == 0 || LocalPosition == CurrentBlock.Length - 1;
        }
    }
}