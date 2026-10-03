using System.Numerics;
using System.Runtime.CompilerServices;
using Zion.Vectors;
using Id = uint;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.Serialization.ADF
{
    internal static class ADFPrimitives
    {
        #region Info
        public static int UsedCount => PrimitivesInfo.Length;

        private static readonly Dictionary<Type, Id> PrimitivesId = new(UsedCount)
        {
            { typeof(bool),       Boolean    },
            { typeof(byte),       Byte       },
            { typeof(sbyte),      SByte      },

            { typeof(short),      Int16      },
            { typeof(int),        Int32      },
            { typeof(long),       Int64      },
            { typeof(ushort),     UInt16     },
            { typeof(uint),       UInt32     },
            { typeof(ulong),      UInt64     },

            { typeof(char),       Char       },
            { typeof(float),      Single     },
            { typeof(double),     Double     },
            { typeof(decimal),    Decimal    },
            { typeof(string),     String     },

            { typeof(Half),       Half       },
            { typeof(Index),      Index      },
            { typeof(Range),      Range      },
            { typeof(BigInteger), BigInteger },

            { typeof(RGBColor),   RGB        },
            { typeof(RGBAColor),  RGBA       },

            { typeof(Vector2),    Vector2    },
            { typeof(Vector2Int), Vector2Int },
            { typeof(Vector3),    Vector3    },
            { typeof(Vector3Int), Vector3Int }
        };

        private static readonly PrimitiveInfo[] PrimitivesInfo =
        [
            PrimitiveInfo.Create<bool>(sizeof(bool), WriteBoolean),
            PrimitiveInfo.Create<byte>(sizeof(byte), WriteByte),
            PrimitiveInfo.Create<sbyte>(sizeof(sbyte), WriteSByte),

            PrimitiveInfo.Create<short>(sizeof(short ), WriteInt16),
            PrimitiveInfo.Create<int>(sizeof(int ), WriteInt32),
            PrimitiveInfo.Create<long>(sizeof(long), WriteInt64),
            PrimitiveInfo.Create<ushort>(sizeof(ushort), WriteUInt16),
            PrimitiveInfo.Create<uint>(sizeof(uint), WriteUInt32),
            PrimitiveInfo.Create<ulong>(sizeof(ulong), WriteUInt64),

            PrimitiveInfo.Create<char>(sizeof(char), WriteChar),
            PrimitiveInfo.Create<float>(sizeof(float), WriteSingle),
            PrimitiveInfo.Create<double>(sizeof(double), WriteDouble),
            PrimitiveInfo.Create<decimal>(sizeof(decimal), WriteDecimal),
            PrimitiveInfo.Create<string>(sizeof(uint), WriteString),

            PrimitiveInfo.Create<Half>(2, WriteHalf),
            PrimitiveInfo.Create<Index>(5, WriteIndex),
            PrimitiveInfo.Create<Range>(10, WriteRange),
            PrimitiveInfo.Create<BigInteger>(4, WriteBigInteger),

            PrimitiveInfo.Create<RGBColor>(3, WriteRGB),
            PrimitiveInfo.Create<RGBAColor>(4, WriteRGBA),

            PrimitiveInfo.Create<Vector2>(8, WriteVector2),
            PrimitiveInfo.Create<Vector2Int>(8, WriteVector2Int),
            PrimitiveInfo.Create<Vector3>(12, WriteVector3),
            PrimitiveInfo.Create<Vector3Int>(12, WriteVector3Int),
        ];

        #endregion

        #region Constants
        public const int Count = 128;

        public const Id Boolean = 0;
        public const Id Byte    = 1;
        public const Id SByte   = 2;

        public const Id Int16   = 3;
        public const Id Int32   = 4;
        public const Id Int64   = 5;
        public const Id UInt16  = 6;
        public const Id UInt32  = 7;
        public const Id UInt64  = 8;

        public const Id Char    = 9;
        public const Id Single  = 10;
        public const Id Double  = 11;
        public const Id Decimal = 12;
        public const Id String  = 13;

        public const Id Half       = 14;
        public const Id Index      = 15;
        public const Id Range      = 16;
        public const Id BigInteger = 17;

        public const Id RGB  = 18;
        public const Id RGBA = 19;

        public const Id Vector2    = 20;
        public const Id Vector2Int = 21;
        public const Id Vector3    = 22;
        public const Id Vector3Int = 23;

        public const Id Object = 127;

        #endregion

        #region PublicMethods
        public static bool IsPrimitive(Id Id)
        {
            return Id < Count;
        }

        public static int SizeOf(Id Id)
        {
            return PrimitivesInfo[Id].Size;
        }

        public static bool TryGetId(Type Type, out Id Id)
        {
            return PrimitivesId.TryGetValue(Type, out Id);
        }

        public static bool TryGetInfo(Type Type, out PrimitiveInfo Info)
        {
            if (PrimitivesId.TryGetValue(Type, out Id Id))
            {
                Info = PrimitivesInfo[Id];
                return true;
            }
            Info = default;
            return false;
        }

        #endregion

        #region WriteMethods
        public static void WriteBoolean(ADFWritingContext Context, StreamGroup Target, bool Value)
        {
            Target.BaseStream.Write(Value);
        }

        public static void WriteByte(ADFWritingContext Context, StreamGroup Target, byte Value)
        {
            Target.BaseStream.Write(Value);
        }

        public static void WriteSByte(ADFWritingContext Context, StreamGroup Target, sbyte Value)
        {
            Target.BaseStream.Write(Value);
        }


        public static void WriteInt16(ADFWritingContext Context, StreamGroup Target, short Value)
        {
            Target.BaseStream.Write(Value);
        }

        public static void WriteInt32(ADFWritingContext Context, StreamGroup Target, int Value)
        {
            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncodedInt(Value);
            }
            else
            {
                Target.BaseStream.Write(Value);
            }
        }

        public static void WriteInt64(ADFWritingContext Context, StreamGroup Target, long Value)
        {
            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncodedInt64(Value);
            }
            else
            {
                Target.BaseStream.Write(Value);
            }
        }

        public static void WriteUInt16(ADFWritingContext Context, StreamGroup Target, ushort Value)
        {
            Target.BaseStream.Write(Value);
        }

        public static void WriteUInt32(ADFWritingContext Context, StreamGroup Target, uint Value)
        {
            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncodedUInt(Value);
            }
            else
            {
                Target.BaseStream.Write(Value);
            }
        }

        public static void WriteUInt64(ADFWritingContext Context, StreamGroup Target, ulong Value)
        {
            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncodedUInt64(Value);
            }
            else
            {
                Target.BaseStream.Write(Value);
            }
        }


        public static void WriteChar(ADFWritingContext Context, StreamGroup Target, char Value)
        {
            Target.BaseStream.Write(Value);
        }

        public static void WriteSingle(ADFWritingContext Context, StreamGroup Target, float Value)
        {
            Target.BaseStream.Write(Value);
        }

        public static void WriteDouble(ADFWritingContext Context, StreamGroup Target, double Value)
        {
            Target.BaseStream.Write(Value);
        }

        public static void WriteDecimal(ADFWritingContext Context, StreamGroup Target, decimal Value)
        {
            Target.BaseStream.Write(Value);
        }

        public static void WriteString(ADFWritingContext Context, StreamGroup Target, string Value)
        {
            var Id = Context.Registries.StringRegistry.GetOrAdd(Value);

            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncodedUInt(Id);
            }
            else
            {
                Target.BaseStream.Write(Id);
            }
        }


        public static void WriteHalf(ADFWritingContext Context, StreamGroup Target, Half Value)
        {
            Target.BaseStream.Write(Value);
        }

        public static void WriteIndex(ADFWritingContext Context, StreamGroup Target, Index Value)
        {
            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncodedIndex(Value);
            }
            else
            {
                Target.BaseStream.Write(Value);
            }
        }

        public static void WriteRange(ADFWritingContext Context, StreamGroup Target, Range Value)
        {
            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncodedIndex(Value.Start);
                Target.BaseStream.Write7BitEncodedIndex(Value.End);
            }
            else
            {
                Target.BaseStream.Write(Value);
            }
        }

        public static void WriteBigInteger(ADFWritingContext Context, StreamGroup Target, BigInteger Value)
        {
            var BaseStream = Target.BaseStream;
            var Stream = Context.Arena.GetStream(1);
            var Link = Reference.CreateNewReference(Target.ChildsLength);

            Stream.Write(Value);
            Target.Add(Stream);

            Target.BaseStream.WriteCompressed(Context, Link);
        }


        public static void WriteRGB(ADFWritingContext Context, StreamGroup Target, RGBColor Value)
        {
            Target.BaseStream.Write(Value);
        }

        public static void WriteRGBA(ADFWritingContext Context, StreamGroup Target, RGBAColor Value)
        {
            Target.BaseStream.Write(Value);
        }


        public static void WriteVector2(ADFWritingContext Context, StreamGroup Target, Vector2 Value)
        {
            Target.BaseStream.Write(Value);
        }

        public static void WriteVector2Int(ADFWritingContext Context, StreamGroup Target, Vector2Int Value)
        {
            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncodedInt(Value.X);
                Target.BaseStream.Write7BitEncodedInt(Value.Y);
            }
            else
            {
                Target.BaseStream.Write(Value);
            }
        }

        public static void WriteVector3(ADFWritingContext Context, StreamGroup Target, Vector3 Value)
        {
            Target.BaseStream.Write(Value);
        }

        public static void WriteVector3Int(ADFWritingContext Context, StreamGroup Target, Vector3Int Value)
        {
            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncodedInt(Value.X);
                Target.BaseStream.Write7BitEncodedInt(Value.Y);
                Target.BaseStream.Write7BitEncodedInt(Value.Z);
            }
            else
            {
                Target.BaseStream.Write(Value);
            }
        }


        internal static void WriteEnum<T>(ADFWritingContext Context, StreamGroup Target, T Value)
        {
            Target.BaseStream.UseSpan
            (
                8, Span => Unsafe.WriteUnaligned(ref Span[0], Value)
            );
        }

        #endregion
    }
}