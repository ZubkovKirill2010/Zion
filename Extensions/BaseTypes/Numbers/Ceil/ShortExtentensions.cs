namespace Zion
{
    public static class ShortExtentensions
    {
        public static bool GetBit(this short Value, int Index)
        {
            return (Value & (1 << Index)) != 0;
        }

        public static short SetBit(this short Value, int Index, bool Bit)
        {
            if (Bit)
            {
                return (short)(Value | (1 << Index));
            }
            else
            {
                return (short)(Value & ~(1 << Index));
            }
        }

        public static bool IsEven(this short Value)
        {
            return (Value & 1) == 0;
        }


        public static bool GetBit(this ushort Value, int Index)
        {
            return (Value & (1 << Index)) != 0;
        }

        public static ushort SetBit(this ushort Value, int Index, bool Bit)
        {
            if (Bit)
            {
                return (ushort)(Value | (1 << Index));
            }
            else
            {
                return (ushort)(Value & ~(1 << Index));
            }
        }

        public static bool IsEven(this ushort Value)
        {
            return (Value & 1) == 0;
        }
    }
}