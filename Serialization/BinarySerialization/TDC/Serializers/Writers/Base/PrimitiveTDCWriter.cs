using System.Numerics;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.Serialization.TDC
{
    public abstract class PrimitiveTDCWriter : BaseTDCWriter
    {
        #region Constructors
        public PrimitiveTDCWriter(BaseTDCWriter Base) : base(Base) { }

        #endregion

        #region PublicMethods
        public void Write(bool Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(byte Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(sbyte Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(char Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(decimal Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(double Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(float Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(int Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(uint Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(long Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(ulong Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(short Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(ushort Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(string Value)
        {
            Write(Value, WriteSimplePrimitive);
        }


        public void Write(Half Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(Index Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(Range Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(BigInteger Value)
        {
            Write(Value, WriteSimplePrimitive);
        }


        public void Write(RGBColor Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(RGBAColor Value)
        {
            Write(Value, WriteSimplePrimitive);
        }


        public void Write(Vector2 Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(Vector2Int Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(Vector3 Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        public void Write(Vector3Int Value)
        {
            Write(Value, WriteSimplePrimitive);
        }

        #endregion

        #region AbstractMethods
        protected abstract void OnWrited(ushort TypeId);

        #endregion

        #region PrivateMethods
        private void Write<T>(T Value, WriteAction<T> WriteAction)
        {
            WriteAction(Value, out ushort TypeId);
            OnWrited(TypeId);
        }

        #endregion
    }
}