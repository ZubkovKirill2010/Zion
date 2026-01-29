//namespace Zion.STP
//{
//    public sealed class Int32Template : TokenTemplate
//    {
//        public override bool Read(StringView String, int Start, out Token Token)
//        {
//            int v = 0x111;
//            if (String.Begins(Start, "0b"))
//            {

//            }
//            if (String.Begins(Start, "0x"))
//            {

//            }

//            int End = String.Skip(Start, Char => char.IsDigit(Char) || Char == '_');
//        }
//    }
//}