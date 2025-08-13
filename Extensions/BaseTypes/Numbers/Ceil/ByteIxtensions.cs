namespace Zion
{
    public static class ByteExtensions
    {
        public static bool GetBit(this byte Value, int Index)
        {
            return (Value & (1 << Index)) != 0;
        }

        public static byte SetBit(this byte Value, int Index, bool Bit)
        {
            if (Bit)
            {
                return (byte)(Value | (1 << Index));
            }
            else
            {
                return (byte)(Value & ~(1 << Index));
            }
        }

        public static bool IsEven(this byte Value)
        {
            return (Value & 1) == 0;
        }


        public static bool GetBit(this sbyte Value, int Index)
        {
            return (Value & (1 << Index)) != 0;
        }

        public static sbyte SetBit(this sbyte Value, int Index, bool Bit)
        {
            if (Bit)
            {
                return (sbyte)(Value | (sbyte)(1 << Index));
            }
            else
            {
                return (sbyte)(Value & ~(1 << Index));
            }
        }

        public static bool IsEven(this sbyte Value)
        {
            return (Value & 1) == 0;
        }
    }
}