namespace Zion
{
    public static class LongExtensions
    {
        public static bool GetBit(this long Value, int Index)
        {
            return (Value & (1 << Index)) != 0;
        }

        public static long SetBit(this long Value, int Index, bool Bit)
        {
            if (Bit)
            {
                return Value | (1 << Index);
            }
            else
            {
                return Value & ~(1 << Index);
            }
        }

        public static bool IsEven(this long Value)
        {
            return (Value & 1) == 0;
        }


        public static bool GetBit(this ulong Value, int Index)
        {
            return (Value & (ulong)(1 << Index)) != 0;
        }

        public static ulong SetBit(this ulong Value, int Index, bool Bit)
        {
            if (Bit)
            {
                return Value | (ulong)(1 << Index);
            }
            else
            {
                return Value & ~(ulong)(1 << Index);
            }
        }

        public static bool IsEven(this ulong Value)
        {
            return (Value & 1) == 0;
        }
    }
}