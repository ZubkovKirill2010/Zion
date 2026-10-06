using System.Runtime.CompilerServices;

namespace Zion.Serialization.ADF
{
    public abstract partial class BaseADFWriter : IDisposable
    {
        #region Data
        private protected readonly ADFWritingContext Context;
        private protected readonly StreamGroup Data;

        #endregion

        #region Properties
        protected ADFWritingOptions Options => Context.Options;

        private protected WritableRegistries   Registries => Context.Registries;
        private protected TypeAssociation TypeAssociation => Context.TypeAssociation;
        private protected WriteStrategies WriteStrategies => Context.WriteStrategies;

        private protected ReferenceIdsRegistry References => Registries.References;
        private protected DataRegistry       DataRegistry => Registries.DataRegistry;
        private protected StringIdRegistry StringRegistry => Registries.StringRegistry;
        private protected FormatRegistry   FormatRegistry => Registries.FormatRegistry;

        public bool IsDisposed { get; private set; }

        public long TotalLength => Data.Length;

        public int CurrentPosition => Data.BaseStream.Position;

        #endregion

        #region Constructors
        internal BaseADFWriter(BaseADFWriter Base) : this(Base.Context) { }

        internal BaseADFWriter(BaseADFWriter Base, StreamGroup Target) : this(Base.Context, Target) { }

        internal BaseADFWriter(ADFWritingContext Context) : this(Context, new(Context?.Arena?.GetStream(1)!)) { }

        internal BaseADFWriter(ADFWritingContext Context, StreamGroup Target)
        {
            this.Context = Context.NotNull();
            this.Data = Target;
        }

        #endregion

        #region AbstractMethods
        protected virtual StreamGroup GetStreamGroup(string Name, in uint NameId, in uint FormatId) => Data;

        protected virtual void OnWrited(string Name, in uint NameId, in uint FormatId) { }

        protected virtual void OnDisposed() { }

        #endregion

        #region PublicMethods
        public void Flush(Stream Destination)
        {
            foreach (var Stream in Data)
            {
                Stream.CopyTo(Destination);
            }
        }

        #endregion

        #region ProtectedMethods
        protected ArenaStream GetNewStream()
        {
            return Context.Arena.GetStream(1);
        }

        protected ArenaStream GetBaseStream()
        {
            return Data.BaseStream;
        }

        protected uint GetOrAddDeferred(Type Type)
        {
            return TypeAssociation.GetOrAddDeferred(Type, FormatRegistry);
        }

        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(BaseADFWriter));
            }
        }

        #endregion

        #region Writing
        public void Write<T>(string Name, T? Value)
        {
            ThrowIfDisposed();

            if (TryWritePrimitive(Name, Value))
            {
                return;
            }

            var NameId = StringRegistry.GetOrAdd(Name.NotNull());

            if (Value is null)
            {
                if (!Options.CanWriteNull)
                {
                    throw new ADFObjectIsNullException(Name);
                }

                var NullFormatId = GetOrAddDeferred(typeof(T));

                GetStreamGroup(Name, in NameId, NullFormatId).BaseStream.WriteCompressedZero(Context);
                OnWrited(Name, in NameId, NullFormatId);
                return;
            }

            var Type = Value.GetType();

            if (TryWriteEnum(Name, in NameId, Type, Value))
            {
                return;
            };
            
            WriteStrategies.GetEntry<T>(Context, Type).Deconstruct
            (
                out var FormatId,
                out var Strategy
            );

            var Target = GetStreamGroup(Name, in NameId, in FormatId);

            Strategy.Write(Context, Target, Value);
            OnWrited(Name, in NameId, in FormatId);
        }

        private bool TryWriteEnum<T>(string Name, in uint NameId, Type Type, T Value)
        {
            if (Type.IsEnum)
            {
                var FormatId = FormatRegistry.Add(DataFormat.GetEnumFormat<T>());
                var Stream = GetStreamGroup(Name, in NameId, in FormatId).BaseStream;

                WriteEnum(Stream, Value);
                OnWrited(Name, in NameId, in FormatId);

                return true;
            }
            return false;
        }

        private void WriteEnum<T>(ArenaStream Stream, T Value)
        {
            Stream.UseSpan
            (
                Unsafe.SizeOf<T>(),
                Span =>
                {
                    Unsafe.WriteUnaligned(ref Span[0], Value);
                }
            );
        }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                Context.CurrentPosition += Data.Length;
                OnDisposed();
            }
        }

        #endregion

        #region PrivateMethods
        private static bool IsRootType(Type Type)
        {
            if (Type.IsValueType)
            {
                return true;
            }
            var BaseType = Type.BaseType;
            return BaseType is null || BaseType == typeof(object);
        }

        private static IEnumerable<Type> EnumerateHierarchy(Type Type)
        {
            if (!IsRootType(Type))
            {
                foreach (var Hierarchy in EnumerateHierarchy(Type.BaseType!))
                {
                    yield return Hierarchy;
                }
            }

            yield return Type;
        }

        #endregion
    }
}