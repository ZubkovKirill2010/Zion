namespace Zion.Serialization.TDC
{
    public abstract class NamedTDCWriter : BaseTDCWriter
    {
        #region Data
        protected readonly DataRegistry DataRegistry;

        #endregion

        #region Constructors
        public NamedTDCWriter(Stream Stream, TypeIdRegistry TypeRegistry, PrimitivesRegistry Primitives, ContainersRegistry Contaienrs)
            : base(Stream, TypeRegistry, Primitives, Contaienrs)
        {
            this.DataRegistry = new();
        }

        #endregion

        #region PublicMethods
        public bool Contains(string Key)
        {
            return DataRegistry.Contains(Key);
        }


        public void Write(string Key, bool Value)
        {
            Write(Key, Value);
        }

        #endregion

        #region AbstractMethods
        protected abstract void Write<T>(string Key, T Value);

        #endregion

        #region PrivateMethods
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