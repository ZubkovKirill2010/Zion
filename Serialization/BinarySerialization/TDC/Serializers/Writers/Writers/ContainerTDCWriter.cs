namespace Zion.Serialization.TDC
{
    public sealed class ContainerTDCWriter : NamedTDCWriter
    {
        #region Constructors
        public ContainerTDCWriter(BaseTDCWriter Base)
            : base(Base) { }

        #endregion

        #region PublicMethods

        #endregion

        #region OverrideMethods
        protected override void OnWrited(string Key, ushort TypeId)
        {
            
        }

        #endregion

        #region PrivateMethods

        #endregion
    }
}