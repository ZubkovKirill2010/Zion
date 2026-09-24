using System.Text.RegularExpressions;

namespace Zion.Serialization.ADF
{
    internal abstract class ClassWriteStrategy<T> : ProvidedWriteStrategy<T>
    {
        public ClassWriteStrategy(ADFWritingContext Context)
            : base(Context) { }


        protected sealed override StreamGroup GetGroupForData(StreamGroup BaseGroup)
        {
            var NewGroup = new StreamGroup(Context.Arena.GetStream(1));
            BaseGroup.Add(NewGroup);
            return NewGroup;
        }

        protected sealed override bool TryWriteReference(ArenaStream BaseStream, T Value)
        {
            if (Context.Registries.References.TryGetReference(Value, out Reference Reference))
            {
                if (Context.Options.Compression)
                {
                    BaseStream.Write7BitEncodedUInt(Reference.Id);
                }
                else
                {
                    BaseStream.Write(Reference.Id);
                }
                return true;
            }
            return false;
        }

        //TODO: Реализовать запись ссылок (чтобы писалась не нулевая позиция)
    }
}