using System.Numerics;

namespace Zion.STP.Dynamic
{
    public sealed partial class RelativeTokenContainer<TPointer> : ITokenContainer<TPointer> where TPointer : TextPointer<TPointer>
    {
        #region Data
        private BaseNode  Main;
        private FinalNode Last;

        private int GroupCount;

        #endregion

        #region Properties
        public int Count { get; private set; }

        public int TokenGroupLength { get; init => field = Math.Max(4, value); }

        #endregion

        #region IContainer
        public void Add(Token Token, TPointer Position)
        {
            Count++;

            if (GroupCount == 0)
            {
                Last = new FinalNode(Position, Token);
                Main = Last;
                GroupCount++;
                return;
            }

            if (Last.Tokens.Count < TokenGroupLength)
            {
                Last.Add(Token);
                return;
            }

            if (BitOperations.IsPow2(GroupCount++))
            {
                Main.Position = Main.Position.Subtract(Position);

                Last = new FinalNode(Position, Token);
                Last.Position = Last.Position.Subtract(Position);

                Main = new Node(Position, Main, Last);
            }
            else
            {
                FinalNode New = new FinalNode(Position, Token);
                Node Parent = FindRightmostEmptySlot((Node)Main);

                if (Parent.Right is null)
                {
                    New.Position = Position.Subtract(Parent.Position);
                    Parent.Right = New;
                }
                else
                {
                    TPointer NewParentPosition = Parent.Position;
                    New.Position = Position.Subtract(NewParentPosition);

                    Node NewParent = new Node(NewParentPosition, Parent.Right, New);
                    Parent.Right = NewParent;
                }

                Last = New;
            }
        }

        public void Clear()
        {
            Main = null;
            Last = null;
            Count = 0;
            GroupCount = 0;
        }

        public void Overwrite(TPointer Start, int RemovedTokens, IEnumerable<Token> Tokens)
        {
            //TODO
        }

        public TPointer GetTokenStart(TPointer Position)
        {
            return default!;
            //TODO
        }

        public IEnumerable<Token> EnumeratorFrom(TPointer Start)
        {
            return default!;
            //TODO
        }

        #endregion

        #region PrivateMethods
        private Node FindRightmostEmptySlot(Node Node)
        {
            while (Node.Right is Node RightNode)
            {
                Node = RightNode;
            }
            return Node;
        }
        
        #endregion
    }
}