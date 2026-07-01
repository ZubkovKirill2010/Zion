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
        public int Count       { get; private set; }
        public int TotalLength { get; private set; }

        public int TokenGroupLength { get; init => field = Math.Max(4, value); }

        #endregion

        #region IContainer
        public void Add(Token Token)
        {
            Count++;

            if (GroupCount == 0)
            {
                Last = new FinalNode(Token);
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
                //TODO: !Пересчитывать абсолютные позиции на относительные!
                Last = new FinalNode(Token);
                Main = new Node(Main, Last);
            }
            else
            {
                FinalNode New = new FinalNode(Token);
                Node Parent = FindRightmostEmptySlot((Node)Main);

                if (Parent.Right is null)
                {
                    Parent.Right = New;
                }
                else
                {
                    Node NewParent = new Node(Parent.Right, New);
                    Parent.Right = NewParent;
                }

                Last = New;
            }
        }

        public void Clear()
        {
            //TODO
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

        }
    }
}