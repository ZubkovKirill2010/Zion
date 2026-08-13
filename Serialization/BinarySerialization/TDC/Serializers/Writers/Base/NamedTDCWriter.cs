using System.Numerics;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.Serialization.TDC
{
    public abstract class NamedTDCWriter : BaseTDCWriter
    {
        #region Data
        protected readonly DataRegistry DataRegistry;

        #endregion

        #region Constructors
        public NamedTDCWriter() : base()
        {
            DataRegistry = new();
        }

        public NamedTDCWriter(BaseTDCWriter Base) : base(Base)
        {
            DataRegistry = new();
        }

        #endregion

        #region PublicMethods
        public bool Contains(string Key)
        {
            return DataRegistry.Contains(Key);
        }


        public void Write<T>(string Key, T Value) where T : ITDCPrimitive<T>
        {
            Write(Key, Value, Value => WritePrimitive(Value, out _));
        }

        public void Write<T>(string Key, ITDCContainer<T> Value)
        {
            CheckKey(Key);

            long Start = Memory.Position;
            WriteContainer(Value);
            long Length = Memory.Position - Start;

            DataRegistry.Add(Key, new(Start, Length));
            OnWrited();
        }


        public void Write(string Key, bool Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, byte Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, sbyte Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, char Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, decimal Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, double Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, float Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, int Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, uint Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, long Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, ulong Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, short Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, ushort Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, string Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }


        public void Write(string Key, Half Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, Index Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, Range Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, BigInteger Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }


        public void Write(string Key, RGBColor Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, RGBAColor Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }


        public void Write(string Key, Vector2 Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, Vector2Int Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, Vector3 Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        public void Write(string Key, Vector3Int Value)
        {
            Write(Key, Value, WriteSimplePrimitive);
        }

        #endregion

        #region AbstractMethods
        protected abstract void OnWrited();

        #endregion

        #region PrivateMethods
        private void Write<T>(string Key, T Value, Action<T> WriteAction)
        {
            CheckKey(Key);

            long Start = Memory.Position;
            WriteAction(Value);
            long Length = Memory.Position - Start;

            DataRegistry.Add(Key, new(Start, Length));
            OnWrited();
        }

        private void CheckKey(string Key)
        {
            if (Contains(Key))
            {
                throw new ArgumentException($"Recording with a key '{Key}' already exists");
            }
        }

        #endregion
    }
}