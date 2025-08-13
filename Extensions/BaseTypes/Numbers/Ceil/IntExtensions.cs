namespace Zion
{
    public static class IntExtensions
    {
        public static bool GetBit(this int Value, int Index)
        {
            return (Value & (1 << Index)) != 0;
        }

        public static int SetBit(this int Value, int Index, bool Bit)
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

        public static bool IsEven(this int Value)
        {
            return (Value & 1) == 0;
        }


        public static bool GetBit(this uint Value, int Index)
        {
            return (Value & (1 << Index)) != 0;
        }

        public static uint SetBit(this uint Value, int Index, bool Bit)
        {
            if (Bit)
            {
                return Value | (uint)(1 << Index);
            }
            else
            {
                return (uint)(Value & ~(1 << Index));
            }
        }

        public static bool IsEven(this uint Value)
        {
            return (Value & 1) == 0;
        }
    }
}